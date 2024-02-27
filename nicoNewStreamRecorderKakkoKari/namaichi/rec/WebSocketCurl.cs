/*
 * Created by SharpDevelop.
 * User: ajkkh
 * Date: 2024/02/14
 * Time: 17:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using namaichi.utility;
using Newtonsoft.Json;
using WebSocket4Net;

namespace namaichi.rec
{
	/// <summary>
	/// Description of WebSocketCurl.
	/// </summary>
	public class WebSocketCurl
	{
		internal string[] webSocketRecInfo;
		internal RecordingManager rm;
		private DateTime lastWebsocketConnectTime = DateTime.MinValue;
		internal WebSocketRecorder wr;
		public bool isRetry = true;
		
		public WebSocketCurl() {}
		public WebSocketCurl(RecordingManager rm, WebSocketRecorder wr, string[] webSocketInfo)
		{
			this.rm = rm;
			this.wr = wr;
			this.webSocketRecInfo = webSocketInfo;
		}
		private IntPtr easy = IntPtr.Zero;
		
		public bool connect() {
			//string mes = "";
			while ((wr == null || wr.isRetry) && isRetry) {
				try {
					var r = connectCore();
					if (!r) {
						onClose();
						Thread.Sleep(6000);
					}
				} finally {
					releaseHandle();
				}
			}
			return true;
		}
		private bool connectCore() {
			isRetry = true;
			lock(this) {
				var  isPass = (TimeSpan.FromSeconds(5) > (DateTime.Now - lastWebsocketConnectTime));
				if (isPass) 
					Thread.Sleep(5000);
				lastWebsocketConnectTime = DateTime.Now;
			}
			
			try {
				var url = webSocketRecInfo[0];
				
				easy = Curl.curl_easy_init();
				
				if (easy == IntPtr.Zero) {
					rm.form.addLogText("ライブラリよりWebSocketへの接続を開始できませんでした");
					return false;
				}
				util.debugWriteLine("curl push connect  ");
				
				Curl.curl_easy_setopt(easy, CURLoption.CURLOPT_URL, url);
				Curl.curl_easy_setopt(easy, CURLoption.CURLOPT_SSL_VERIFYPEER, 0);
				Curl.curl_easy_setopt(easy, CURLoption.CURLOPT_CONNECT_ONLY, 2L);
				Curl.curl_easy_setopt(easy, CURLoption.CURLOPT_USERAGENT, util.userAgent);
				
				easy = setHeader(easy);
				
				var code = Curl.curl_easy_perform(easy);
				util.debugWriteLine("curl ws connect code " + code);
				if(code != CURLcode.CURLE_OK) {
					util.debugWriteLine("curl easy error " + code + " " + url);
					rm.form.addLogText("WebSocketの接続に失敗しました " + code);
					stop();
					Thread.Sleep(5000);
					return false;
				} else {
					Thread.Sleep(1000);
					onOpen();
					
					var buf = "";
					while ((wr == null || wr.isRetry) && isRetry) {
						IntPtr recvPtr = IntPtr.Zero;
						try {
							uint recvN = 0;
							//Thread.Sleep(1000);
							var wsFramePtr = IntPtr.Zero;
							var recvBytes = new byte[100000];
							
							CURLcode recvCode;
							recvPtr = Curl.curl_ws_recv_wrap(easy, out recvCode, out recvN);
							if (recvCode != CURLcode.CURLE_OK)
								util.debugWriteLine("curl ws recvCode not ok " + recvCode);
							
							var recvNI = (int)recvN;
							Marshal.Copy(recvPtr, recvBytes, 0, recvNI);
							
							var recvS = Encoding.UTF8.GetString(recvBytes, 0, recvNI);
							if (string.IsNullOrEmpty(recvS)) {
								Thread.Sleep(1000);
								continue;
							}
							
							if (isErrorMes(recvS)) {
								buf += recvS;
								if (isErrorMes(buf))
									continue;
							} else buf = recvS;
							
							onMessageReceive(buf);
							buf = "";
							//onMessageReceiveCore(recvS);
						} catch (Exception e) {
							util.debugWriteLine(e.Message + e.Source + e.StackTrace);
						} finally {
							if (recvPtr != IntPtr.Zero) Curl.memFree(recvPtr);
						}
						
					}
			    }
				//Curl.curl_easy_cleanup(easy);
				
				return true;
				
			} catch (Exception ee) {
				util.debugWriteLine("push connect exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				return false;
			}
		}
		public void wsSend(string mes) {
			util.debugWriteLine("ws send curl " + mes);
			if (easy == IntPtr.Zero) {
				util.debugWriteLine("curl ws send no easy " + mes);
				return;
			}
			int outN = 0;
			var sendCode = Curl.curl_ws_send_wrap(easy, mes, mes.Length, out outN, 0, (int)curlWsFlags.CURLWS_TEXT);
			util.debugWriteLine("curl ws send code " + sendCode + " " + mes);
		}
		virtual public void stop() {
			//wr.IsRetry = false;
			isRetry = false;
			releaseHandle();
		}
		void releaseHandle() {
			try {
				lock(this) {
					if (easy != IntPtr.Zero) {
						Curl.curl_easy_cleanup(easy);
						easy = IntPtr.Zero;
					}
				}
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
		}
		void onClose() {
			util.debugWriteLine("on close wscurl");
			if (wr == null) return;
			
			Task.Run(() => {
			    wr.endProgramCheck();
			});
			if (rm.rfu != wr.rfu || wr.isEndProgram) {
				wr.isRetry = false;
			}
		}
		virtual internal void onOpen() {
			var mes = webSocketRecInfo[1]; 
			wsSend(mes);
		}
		virtual internal void onMessageReceive(string recvS) {
			util.debugWriteLine("receive curlws " + recvS);
			wr.onMessageReceiveCore(recvS);
		}
		virtual internal IntPtr setHeader(IntPtr easy) {
			return easy;
		}
		virtual internal bool isErrorMes(string recvS) {
			return false;
		}
	}
}
