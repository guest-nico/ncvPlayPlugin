/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/04/16
 * Time: 0:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using WebSocket4Net;
using System.Security.Authentication;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
//using Newtonsoft.Json;
using System.Threading;
using System.Drawing;
using namaichi.info;

namespace namaichi.rec
{
	/// <summary>
	/// Description of WebSocketRecorder.
	/// </summary>
	public class WebSocketRecorder : IRecorderProcess
	{
		private string[] webSocketInfo;
		private string broadcastId;
		private string userId;
		private string lvid;
		private bool isPremium = false;
		private string programType;
		private CookieContainer container;
		private string[] recFolderFile;
		private RecordingManager rm;
		private RecordFromUrl rfu;
		public Html5Recorder h5r;
		private WebSocket ws;
		private WebSocket wsc;
		private Record rec;
		private StreamWriter commentSW;
		//public string msUri;
		//public string[] msReq;
		private long serverTime;
		private string ticket;
		private bool isRetry = true;
		private string msThread;
		private string sendCommentBuf = null;
		private bool isSend184 = true;
		
		private bool isNoPermission = false;
		//public long openTime;
		public long _openTime;
		public bool isEndProgram = false;
		public int lastSegmentNo = -1;
		//public bool isTimeShift = false;
		private TimeShiftConfig tsConfig = null;
		private bool isTimeShiftCommentGetEnd = false;
		private DateTime lastEndProgramCheckTime = DateTime.Now;
		private DateTime lastWebsocketConnectTime = DateTime.Now;
		
		private TimeSpan jisa;
		//private DateTime beginTime = null;
		//private DateTime endTime = null;
		private TimeSpan programTime = TimeSpan.Zero;
		
//		public DateTime tsHlsRequestTime;
//		public TimeSpan tsStartTime;
			
		private WebSocket[] himodukeWS = new WebSocket[2];
			
//		private System.Threading.Thread mainThread;
		//public TimeShiftCommentGetter tscg = null;
		
		bool isWaitNextConnection = false;
		List<WebSocket> wsList = new List<WebSocket>();
		
		private List<string> debugWriteBuf = new List<string>();
		private Task tsWriterTask = null;
		private bool isSub;
		public bool isRtmp;
		private RtmpRecorder rr;
		
		private string qualityRank = null;
		private string isGetComment = null;
		private string isGetCommentXml = null;
		private string commentFileName = null;
//		private string commentHead = null;
		private string engineMode = null;
		
		private bool isXmlComment = true;
		//private XmlCommentGetter_ontime xcg = null;
		//private TimeShiftCommentGetter_xml tscgx = null;
		private string selectQuality = null;
		private string selectLatency = null;
		
		public WebSocketRecorder(string[] webSocketInfo, 
				CookieContainer container, string[] recFolderFile, 
				RecordingManager rm, RecordFromUrl rfu, 
				Html5Recorder h5r, long openTime, 
				int lastSegmentNo, bool isTimeShift, string lvid, 
				TimeShiftConfig tsConfig, string userId, 
				bool isPremium, TimeSpan programTime, 
				string programType, long _openTime, bool isSub, bool isRtmp,
				string latency
			)
		{
			this.webSocketInfo = webSocketInfo;
			this.container = container;
			this.recFolderFile = recFolderFile;
			this.rm = rm;
			this.rfu = rfu;
			this.h5r = h5r;
			this.openTime = openTime;
			this.lastSegmentNo = lastSegmentNo;
			this.isTimeShift = isTimeShift;
			this.lvid = lvid;
			this.tsConfig = tsConfig;
			this.userId = userId;
			this.isPremium = isPremium;
			this.programTime = programTime;
			isJikken = false;
			this.programType = programType;
			this._openTime = _openTime;
			this.isSub = isSub;
			this.isRtmp = isRtmp;
			
			this.qualityRank = rm.cfg.get("qualityRank");
			this.isGetComment = rm.cfg.get("IsgetComment");
			this.isGetCommentXml = rm.cfg.get("IsgetcommentXml");
			this.engineMode = rm.cfg.get("EngineMode");
			selectLatency = latency;
		}
		public bool start(bool isRtmpOnlyPage) {
			addDebugBuf("ws rec start");
			
			isXmlComment = false;
			tsWriterTask = Task.Run(() => {startDebugWriter();});
			
//			connect(webSocketInfo[0]);
			if (isRtmpOnlyPage) {
				Task.Run(() => {
					rr = new RtmpRecorder(lvid, container, rm, rfu, !isRtmp, recFolderFile, this, openTime);
					Task.Run(() => {
						rr.record(null, null);
						rm.hlsUrl = "end";
						if (rr.isEndProgram) isEndProgram = true;
						isRetry = false;
					});
				});
			} else {
				connect();
				broadcastId = util.getRegGroup(webSocketInfo[0], "watch/.*?(\\d+?)(\\?|/)");
			}
			
			/*
			if (isRtmp || 
			    (rm.cfg.get("IsHokan") == "true" && 
			     !rfu.isRtmpMain && !rm.isPlayOnlyMode && 
			     !rfu.isSubAccountHokan && engineMode == "0" && !isTimeShift)) {
				rfu.subGotNumTaskInfo = new List<numTaskInfo>();
				rr = new RtmpRecorder(lvid, container, rm, rfu, !isRtmp, recFolderFile, this, openTime);
				Task.Run(() => {
					rr.record();
					rm.hlsUrl = "end";
					if (rr.isEndProgram) isEndProgram = true;
					isRetry = false;
				});
				/*
				Task.Run(() => {
				    xcg = new XmlCommentGetter(lvid, container, rm, rfu, recFolderFile[1], this, isTimeShift, isRtmp, openTime, _openTime, serverTime);
				    xcg.get();
				});
				while (rm.rfu == rfu && isRetry) Thread.Sleep(1000);
				*
				
			}
			*/
			
			/*
			if (!isRtmpOnlyPage) {
				connect();
				addDebugBuf("rm.rfu dds1 " + rm.rfu);
			
				broadcastId = util.getRegGroup(webSocketInfo[0], "watch/.*?(\\d+?)(\\?|/)");
			}
			*/
//			userId = util.getRegGroup(webSocketInfo[0], "audience_token=.+?_(.+?)_");
			
			addDebugBuf("rm.rfu dds6 " + rm.rfu);
			
			addDebugBuf("ws main " + ws + " a " + (ws == null));
			
			
			
//			while (ws.State != WebSocket4Net.WebSocketState.Closed) {
//			while (rm.rfu == rfu && ws != null && isRetry && 
//			       (rec == null || !rec.isStopRead())) {
			while (rm.rfu == rfu && isRetry) {
				/*
				if (isTimeShift && rm.rfu == rfu && 
				    	isGetComment == "true" && engineMode == "3" &&
				    	tscg != null && tscg.isEnd) {
					break;
				}
				*/
//				if (rec != null) 
//					addDebugBuf("isStopread " + rec.isStopRead());
				
				
				if (!isRtmp && ws.State == WebSocket4Net.WebSocketState.Closed) {
					addDebugBuf("no connect loop ws close");
//					connect();
				}
				
//				if (DateTime.Now > DateTime.Parse("2018/10/19 4:43")) resetWebsocketInfo();
				//test
//				GC.Collect();
//				GC.WaitForPendingFinalizers();
				
				System.Threading.Thread.Sleep(1000);
			}
//			while (isTimeShift && rm.rfu == rfu) 
//				System.Threading.Thread.Sleep(300);
			
			
			//util.debugWriteLine("loop end rm.rfu " + rm.rfu.GetHashCode() + " " + rfu.GetHashCode() + " isretry " + isRetry);
			
			isRetry = false;
			if (rr != null) rr.retryMode = (isEndProgram) ? 2 : 1;
//			if (rm.rfu != rfu && tscg != null) tscg.setIsRetry(false);
			
//			if (isTimeShift && rm.rfu == rfu && tscg != null) {
			
			
			if (rm.rfu != rfu) {
				//if (rr != null) rr.isRetry = false;
				stopRecording(ws, wsc);
//				ws.Close();
//				wsc.Close();
				if (rec != null) 
					rec.waitForEnd();
			}
			if (!isRetry) {
				//if (rr != null) rr.isRetry = false;
				stopRecording(ws, wsc);
				if (rec != null)
					rec.waitForEnd();
			}
			
			
			addDebugBuf("closed saikai");
			
			return isNoPermission;
		}
		private bool connect() {
			lock (this) {
				var  isPass = (TimeSpan.FromSeconds(1) > (DateTime.Now - lastWebsocketConnectTime));
				lastWebsocketConnectTime = DateTime.Now;
				if (isPass) 
					Thread.Sleep(1000);
			}
			if (isWaitNextConnection) {
				Thread.Sleep(90000);
				resetWebsocketInfo();
				isWaitNextConnection = false;
				addDebugBuf("after wait reset  " + " wsList " + wsList.Count);
			}
			
			if (ws != null)
				addDebugBuf("ws connect " + ws.GetHashCode());
			try {
				//ws = new WebSocket(webSocketInfo[0]);
				addDebugBuf("ws connect webSocketInfo[0] " + webSocketInfo[0] + " wsList " + wsList.Count);
				//ws = new WebSocket(webSocketInfo[0], "", null, null, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36", "", WebSocketVersion.Rfc6455, null, SslProtocols.Tls12);
				ws = new WebSocket(webSocketInfo[0], "", null, null, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36", "", WebSocketVersion.Rfc6455, null, SslProtocols.None);
				ws.Opened += onOpen;
				ws.Closed += onClose;
				ws.DataReceived += onDataReceive;
				ws.MessageReceived += onMessageReceive;
				ws.Error += onError;
				
				ws.Open();
			} catch (Exception ee) {
				addDebugBuf("ws connect exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				return false;
			}
			
			var _ws = ws;
			try {
				wsList.Add(ws);
				
//				var _ws = ws; 
				Thread.Sleep(5000);
				if (_ws != null && _ws.State == WebSocketState.Connecting) {
					addDebugBuf("ws connect 5 seconds close");
					try {
						_ws.Close();
					} catch (Exception e) {
						addDebugBuf("connect timeout ws exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
					}
				}
				
			} catch (Exception ee) {
				try {
					ws.Close();
					_ws.Close();
				} catch (Exception eee) {
					addDebugBuf("ws connect exception " + eee.Message + eee.Source + eee.StackTrace + eee.TargetSite);
				}
				addDebugBuf("ws connect exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				return false;
			}
			return true;
		}
		private void onOpen(object sender, EventArgs e) {
			addDebugBuf("on open rm.rfu dds2 " + rm.rfu + " ws " + sender.GetHashCode() + " wsList " + wsList.Count);
			
			if (sender != ws) {
				((WebSocket)sender).Close();
				addDebugBuf("hukusuu ws close");
			}
			
			if (isNoPermission) {
				webSocketInfo[1] = webSocketInfo[2] == "1" ?
					webSocketInfo[1].Replace("\"requireNewStream\":false", "\"requireNewStream\":true")
					: webSocketInfo[1].Replace("\"reconnect\":true", "\"reconnect\":false");
			}
							
			//String leoReq = "{\"type\":\"watch\",\"body\":{\"command\":\"playerversion\",\"params\":[\"leo\"]}}";
			//addDebugBuf("leoReq " + leoReq);
			//addDebugBuf("websocketinfo1 " + webSocketInfo[1]);
			//ws.Send(leoReq);
			
			
			sendMessage(ws, webSocketInfo[1]);
			
			
			/*
			if (isNoPermission)
				ws.Send(webSocketInfo[1]);
			else ws.Send(webSocketInfo[1].Replace("\"requireNewStream\":false", "\"requireNewStream\":true"));
			*/
			addDebugBuf("open send  " + ws);
			addDebugBuf("rm " + rm + " rm.rfu " + rm.rfu + " rfu " + rfu);
			
//			if (rm.rfu != rfu) stopRecording();
		}
		private void onClose(object sender, EventArgs e) {
			addDebugBuf("on close " + e.ToString() + " ws hash " + sender.GetHashCode() + " istimeshift " + isTimeShift + " wsList " + wsList.Count);
			try {
				wsList.Remove((WebSocket)sender);
			} catch (Exception ee) {util.debugWriteLine(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);}
			addDebugBuf("on close2 " + " wsList " + wsList.Count);
			
			Task.Run(() => {  
				if (!isTimeShift && isEndedProgram()) {
					isRetry = false;
					if (rr != null) rr.retryMode = 2;
					//if (tscg != null) tscg.setIsRetry(false);
					isEndProgram = true;
				}
			});
			
			//stopRecording();
			if (rm.rfu == rfu && !isEndProgram && (WebSocket)sender == ws) {
				while (true) {
					try {
						if (!connect()) continue;
						break;
					} catch (Exception ee) {
						addDebugBuf(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
					}
				}
//				ws.Open();
			}
			

		}
		private void onError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e) {
			addDebugBuf("on error " + e.Exception.Message + " ws " + sender.GetHashCode());
			//stopRecording();
//			reConnect();
//			ws.Open();
//			endStreamProcess();
		}
		private void onDataReceive(object sender, DataReceivedEventArgs e) {
			addDebugBuf("on data " + e.Data);
		}
		private void onMessageReceive(object sender, MessageReceivedEventArgs e) {
			addDebugBuf("receive " + e.Message);
			var type = util.getRegGroup(e.Message, "\"" + (webSocketInfo[2] == "1" ? "command" : "type") + "\":\"(.+?)\"");
			
			if (sender != ws) {
				((WebSocket)sender).Close();
				addDebugBuf("hukusuu ws close");
			}
			
//			addDebugBuf("ws " + ws);
			if (ws == null) return;
			//pong
			if (type == "ping") {
				sendPong();
			}
			
			//get message
			if (type == "room" || type == "messageServerUri") {
				if (isSub) return;
				
				
			}
			
			//record
			if (type == "stream" || type == "currentstream") {
				//addDebugBuf("mediaservertype = " + util.getRegGroup(e.Message, "(\"mediaServerType\".\".+?\")"));
				if (isRtmp) {
					//rm.form.setQualityList(new string[]{"RTMP"}, "rtmp");
					//return;
				}
//				if (engineMode == "3") return;
				
				string[] gettableList = null;
				var bestGettableQuolity = getBestGettableQuolity(e.Message, out gettableList);
				var currentQuality = util.getRegGroup(e.Message, "\"quality\":\"(.+?)\"");
				//var gettableList = util.getRegGroup(e.Message, "\"availableQualities\"\\:\\[(.+?)\\]").Replace("\"", "").Split(',');
				
				int listCount = 0;
				string listText = "";
				rm.form.getQualityListInfo(out listCount, out listText);
				
				if (!(isRtmp && isTimeShift)) {
					if (listText == "" ||
					    	Array.IndexOf(gettableList, listText) == -1) {
						if (isFirstChoiceQuality(currentQuality, bestGettableQuolity)) {
							rm.form.setQualityList(gettableList, currentQuality);
							if (isRtmp) {
								rtmpRecord(e.Message, currentQuality);
							} else {
								record(e.Message, currentQuality);
							}
						} else 
							sendUseableStreamGetCommand(bestGettableQuolity);
					} else {
						if (currentQuality == listText) {
							
							if (isRtmp) {
								rtmpRecord(e.Message, currentQuality);
							} else {
								record(e.Message, currentQuality);
							}
						} else 
							sendUseableStreamGetCommand(listText);
					}
				}
				
			}
			
			//new stream retry
			if (e.Message.IndexOf("\"NO_PERMISSION\"") >= 0
			    || e.Message.IndexOf("\"TAKEOVER\"") >= 0
			    || e.Message.IndexOf("\"SERVICE_TEMPORARILY_UNAVAILABLE\"") >= 0
			   	|| e.Message.IndexOf("\"END_PROGRAM\"") >= 0
			    || e.Message.IndexOf("\"TOO_MANY_CONNECTIONS\"") >= 0
			    || e.Message.IndexOf("\"TEMPORARILY_CROWDED\"") >= 0
			    || e.Message.IndexOf("\"CROWDED\"") >= 0
			   	|| e.Message.IndexOf("\"CONNECT_ERROR\"") >= 0
			   	|| e.Message.IndexOf("\"NO_STREAM_AVAILABLE\"") > -1) {
				if (e.Message.IndexOf("\"TAKEOVER\"") >= 0 && !isRtmp) rm.form.addLogText("追い出されました。");
				
				//SERVICE_TEMPORARILY_UNAVAILABLE 予約枠開始後に何らかの問題？
				if (e.Message.IndexOf("\"SERVICE_TEMPORARILY_UNAVAILABLE\"") > 0 && !isRtmp) 
					rm.form.addLogText("サーバーからデータの受信ができませんでした。リトライします。");
				
				if (e.Message.IndexOf("\"END_PROGRAM\"") > 0 || e.Message.IndexOf("\"NO_STREAM_AVAILABLE\"") > -1) {
					isEndProgram = true;
					isRetry = false;
					if (rr != null) rr.retryMode = 2;
					//if (tscg != null) tscg.setIsRetry(false);
				}
				
//				connect(webSocketInfo[0].Replace("\"requireNewStream\":false", "\"requireNewStream\":true"));
				isNoPermission = true;
				if (e.Message.IndexOf("\"TEMPORARILY_CROWDED\"") >= 0 ||
				    	e.Message.IndexOf("\"CONNECT_ERROR\"") >= 0) {
					isWaitNextConnection = true;
					//{"type":"error","body":{"code":"CONNECT_ERROR"}}
					
					if ((e.Message.IndexOf("\"TEMPORARILY_CROWDED\"") >= 0 || e.Message.IndexOf("CROWDED") > -1) && !isRtmp)
						rm.form.addLogText("満員でした");
					
					if (e.Message.IndexOf("\"CONNECT_ERROR\"") >= 0 && !isRtmp)
						rm.form.addLogText("接続エラーでした");
					#if DEBUG
					#endif
					
				//stopRecording();
//				reConnect();
//				Task.Run(() => {
//				         	sendIntervalPong();
//				         });
				}
			} else if (e.Message.IndexOf("\"INTERNAL_SERVERERROR\"") >= 0) {
				try {
					ws.Close();
				} catch (Exception ee) {
					addDebugBuf("notify ws close exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				}
			} else if (type == "disconnect") {
				addDebugBuf("unknown disconnect");
				isNoPermission = true;
				//stopRecording();
//				reConnect();
			}
			if (type == "statistics") {
				if (isSub) return;
				
				//displayStatistics(e.Message);
//				Task.Run(() => {
//				         	sendIntervalPong();
//				         });
			}
			if (e.Message.IndexOf("\"notifyType\":\"reconnect\"") >= 0) {
				//{"type":"notify","body":{"notifyType":"reconnect","audienceToken":"8269898711679_225832_1539687621_2779cc2ce649ffb2d8aad1ac5a90d025ee83b8b4","reconnectWaitTime":1}}
				var waitTime = util.getRegGroup(e.Message, "\"reconnectWaitTime\":(\\d+)");
				if (waitTime == null) return;
				Thread.Sleep(int.Parse(waitTime) * 1000);
				
				try {
					ws.Close();
				} catch (Exception ee) {
					addDebugBuf("notify ws close exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				}
				#if DEBUG
					if (!isRtmp)
						rm.form.addLogText("notify reconnect");
				#endif
				
				
			}
			if (type == "serverTime" || type == "servertime") {
				if (webSocketInfo[2] == "1") {
					var _t = (int)(long.Parse(util.getRegGroup(e.Message, "(\\d+)")) / 1000);
					jisa = util.getUnixToDatetime(_t) - DateTime.Now;
				} else {
					var _t = DateTime.Parse(util.getRegGroup(e.Message, "\"currentMs\":\"(.+?)\""));
					jisa = _t - DateTime.Now;
				}
				
			}
			if (type == "schedule") {
			
				//if (isSub) return;
				DateTime beginTime, endTime;
				if (webSocketInfo[2] == "1") {
					var _beginTime = (int)(long.Parse(util.getRegGroup(e.Message, "\"begintime\":(\\d+)")) / 1000);
					var _endTime = (int)(long.Parse(util.getRegGroup(e.Message, "\"endtime\":(\\d+)")) / 1000);
					beginTime = util.getUnixToDatetime(_beginTime);
					endTime = util.getUnixToDatetime(_endTime);
				} else {
					beginTime = DateTime.Parse(util.getRegGroup(e.Message, "\"begin\":\"(.+?)\""));
					endTime = DateTime.Parse(util.getRegGroup(e.Message, "\"end\":\"(.+?)\""));
				}
				programTime = endTime - beginTime;
				
				//if (!isTimeShift)
//					displaySchedule();
				if (util.isStdIO) {
					Console.WriteLine("info.startTime:" + beginTime.ToString("MM/dd(ddd) HH:mm:ss"));
					Console.WriteLine("info.endTime:" + endTime.ToString("MM/dd(ddd) HH:mm:ss"));
					Console.WriteLine("info.programTime:" + programTime.ToString("h'時間'mm'分'ss'秒'"));
				}
			}
			if (type == "postkey") {
				
			}
			                      
		}
		private void sendPong() {
	    	try {
				if (webSocketInfo[2] == "1") {
					var dt = System.DateTime.Now.ToShortTimeString();
					sendMessage(ws, "{\"body\":{},\"type\":\"pong\"}");
					sendMessage(ws, "{\"type\":\"watch\",\"body\":{\"command\":\"watching\",\"params\":[\"" + broadcastId + "\",\"-1\",\"0\"]}}");
				} else {
					sendMessage(ws, "{\"type\":\"pong\"}");
					sendMessage(ws, "{\"type\":\"keepSeat\"}");
				}
			} catch (Exception e) {
				addDebugBuf(e.Message+e.StackTrace);
			}
		}
		private void record(String message, string currentQuality) {
			string hlsUrl = (isRtmp) ? 
				(util.getRegGroup(message, "\"uri\":\"(.+?)\"") + "/" + util.getRegGroup(message, "\"name\":\"(.+?)\""))
				: util.getRegGroup(message, "\"uri\":\"(.+?)\"");
			addDebugBuf("rec " + string.Join(" ", recFolderFile));
			//rm.hlsUrl = hlsUrl;
			addDebugBuf(hlsUrl);
				
			if (rec == null) {
				rec = new Record(rm, true, rfu, hlsUrl, recFolderFile[1], lastSegmentNo, container, isTimeShift, this, lvid, tsConfig, openTime, ws, recFolderFile[2], isSub);
				Task.Run(() => {
			   		
					rec.record(currentQuality, isRtmp);
					if (rec.isEndProgram) {
						addDebugBuf("stop websocket recd");
						isRetry = false;
						if (rr != null) rr.retryMode = 2;
//						if (tscg != null) tscg.setIsRetry(false);
						isEndProgram = true;
			
					}
				});
         	} else {
		    	rec.reSetHlsUrl(hlsUrl, currentQuality, ws, isRtmp);
         	}
				 
//				stopRecording();
			
		}
		
		public void stopRecording(WebSocket _ws, WebSocket _wsc) {
			addDebugBuf("stop recording");
			try {
				if (_ws != null && _ws.State != WebSocketState.Closed) {
					try {
						_ws.Close();
					} catch (Exception e) {
						addDebugBuf("stoprecording ws close exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
					}
				}
//				_ws = null;
			} catch (Exception e) {
				addDebugBuf("ws close error");
				addDebugBuf(e.Message + e.StackTrace);
			}
			try {
				if (_wsc != null && _wsc.State != WebSocketState.Closed && _wsc.State != WebSocketState.Closing) {
					addDebugBuf("state close wsc " + _wsc.State);					
					_wsc.Close();
				}
			} catch (Exception e) {
				addDebugBuf("wsc close error");
				addDebugBuf(e.Message + e.StackTrace);
			}
			try {
//				if (commentSW != null) commentSW.Close();
			} catch (Exception e) {
				addDebugBuf("comment sw close error");
				addDebugBuf(e.Message + e.StackTrace);
			}
			/*
			try {
				if (rec != null) rec.stopRecording();
			} catch (Exception e) {
				addDebugBuf("rec close error");
				addDebugBuf(e.Message + e.StackTrace);
			}
			isRetry = false;
			*/
		}
		/*
		private void endStreamProcess() {
			if (rm.rfu != rfu) return;
					
			//string[] recFolderFile = util.getRecFolderFilePath(recFolderFile[0], recFolderFile[1], recFolderFile[2], recFolderFile[3], recFolderFileInfo[4]);
			addDebugBuf("recforlderfie");
			addDebugBuf("recforlderfi " + string.Join(" ",recFolderFile));
			if (recFolderFile == null) return;
			
			if (!h5r.isAliveStream()) return;
			start();
		}
		*/
		
		private bool isFirstChoiceQuality(string currentQuality, string bestGettableQuolity) {
//			var bestGettableQuality = getBestGettableQuolity(message);
			
			
			
			return currentQuality == bestGettableQuolity; 
			
		}
		private string getBestGettableQuolity(string msg, out string[] gettableList) {
			var qualityList = new List<string>{//"abr",
				"super_high", "high",
				"normal", "low", "super_low",
				"audio_high"};
			
			gettableList = webSocketInfo[2] == "1" ? 
					util.getRegGroup(msg, "\"qualityTypes\"\\:\\[(.+?)\\]").Replace("\"", "").Split(',')
					: util.getRegGroup(msg, "\"availableQualities\"\\:\\[(.+?)\\]").Replace("\"", "").Split(',');
			var ranks = (rm.ri == null) ? (qualityRank.Split(',')) :
					rm.ri.qualityRank;
			//if (ranks.Length == 6) qualityList.Insert(0, "abr");
			gettableList = new List<string>(gettableList).Where(x => x != "abr").ToArray();
			
			var bestGettableQuality = "normal";
			foreach(var r in ranks) {
				var q = qualityList[int.Parse(r)];
				if (gettableList.Contains(q) && q != "abr") {
					bestGettableQuality = q;
					break;
				}
			}
			return bestGettableQuality;
		}
		private void sendUseableStreamGetCommand(string bestGettableQuolity) {
			util.debugWriteLine("sendUseableStream " + bestGettableQuolity);
			var req = "";
			
			if (webSocketInfo[2] == "1") {
				req = (isRtmp) ?
						("{\"type\":\"watch\",\"body\":{\"command\":\"getstream\",\"requirement\":{\"protocol\":\"rtmp\",\"quality\":\"" + bestGettableQuolity + "\"}}}")
						: ("{\"type\":\"watch\",\"body\":{\"command\":\"getstream\",\"requirement\":{\"protocol\":\"hls\",\"quality\":\"" + bestGettableQuolity + "\",\"isLowLatency\":false}}}");
			} else {
				var latency = float.Parse(selectLatency) < 1.1 ? "low" : "high";
				req = "{\"type\":\"changeStream\",\"data\":{\"quality\":\"" + bestGettableQuolity + "\",\"protocol\":\"hls\",\"latency\":\"" + latency + "\",\"chasePlay\":false}}";
			}
			
				   
			
			sendMessage(ws, req);
		}
		override public void reConnect() {
			addDebugBuf("reconnect wr");
//			onOpen(null, null);
			try {
				ws.Close();
			} catch (Exception e) {
				addDebugBuf("reconnect ws exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
//			ws.Open();
		}
		public void reConnect(WebSocket _ws) {
			addDebugBuf("reconnect " + _ws + " " + _ws.GetHashCode() + " ws " + ws.GetHashCode());
			try {
				ws.Close();
			} catch (Exception e) {
				addDebugBuf("reconnect ws exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
		}
		public bool isEndedProgram() {
			var isPass = (DateTime.Now - lastEndProgramCheckTime < TimeSpan.FromSeconds(5));
			addDebugBuf("ispass " + isPass + " lastendprogramchecktime " + lastEndProgramCheckTime);
			if (isPass) return false;
			lastEndProgramCheckTime = DateTime.Now;
			
			var a = new System.Net.WebHeaderCollection();
			var res = util.getPageSource(h5r.url, ref a, container);
			addDebugBuf("isendedprogram url " + h5r.url + " res==null " + (res == null));
			if (res == null) return false;
			if (res.IndexOf("user.login_status = 'not_login'") > -1) {
				addDebugBuf("isendprogram not login");
				var cg = new CookieGetter(rm.cfg);
				var cgTask = cg.getHtml5RecordCookie(h5r.url, isSub);
				cgTask.Wait();
				container = cgTask.Result[0];
				res = util.getPageSource(h5r.url, container, null, false, 5000);
				res = System.Web.HttpUtility.HtmlDecode(res);
				var _webSocketInfo = h5r.getWebSocketInfo(res, isRtmp, rm.form, rm.form.getLatencyText(), rfu.wssUrl);
				isNoPermission = true;
				addDebugBuf("isendprogram login websocketInfo " + webSocketInfo[0] + " " + webSocketInfo[1]);
				if (_webSocketInfo[0] == null || _webSocketInfo[1] == null) {
					addDebugBuf(res);
				} else webSocketInfo = _webSocketInfo;
				return false;
			}
			if (res == null) return false;
			var type = util.getPageType(res);
			addDebugBuf("is ended program  pagetype " + type);
			var isEnd = (type == 7 || type == 2 || type == 3 || type == 9);
			return isEnd;
		}
		
		private void startDebugWriter() {
			#if !DEBUG
				return;
			#endif
			while ((rm.rfu == rfu && isRetry) || debugWriteBuf.Count > 0) {
				try {
					lock (debugWriteBuf) {
						string[] l = new String[debugWriteBuf.Count + 10];
						debugWriteBuf.CopyTo(l);
						//var l = new List<string>(debugWriteBuf);
						//string[] _l = debugWriteBuf.ToArray();
						//var l = new List<string>(_l.li);
	//					util.debugWriteLine("debugwritebuf count " + debugWriteBuf.Count);
//						var l = debugWriteBuf.ToList<string>();
						foreach (var b in l) {
							if (b == null) continue;
							util.debugWriteLine(b + " " + util.getMainSubStr(isSub, true));
							debugWriteBuf.Remove(b);
						}
					}
					Thread.Sleep(500);
				} catch (Exception e) {
					addDebugBuf("debug writer exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
				}
			}
		}
		private void addDebugBuf(string s) {
			#if !DEBUG
				return;
			#endif
			var dt = DateTime.Now.ToLongTimeString();
			debugWriteBuf.Add(dt + " " + s);
		}
		private void resetWebsocketInfo() {
			try {
				var cg = new CookieGetter(rm.cfg);
				var cgTask = cg.getHtml5RecordCookie(h5r.url, isSub);
				cgTask.Wait();
				container = cgTask.Result[0];
				var res = util.getPageSource(h5r.url, container, null, false, 5000);
				res = System.Web.HttpUtility.HtmlDecode(res);
				var _webSocketInfo = h5r.getWebSocketInfo(res, isRtmp, rm.form, rm.form.getLatencyText(), rfu.wssUrl);
				isNoPermission = true;
				addDebugBuf("resetWebsocketInfo " + _webSocketInfo[0] + " " + _webSocketInfo[1]);
				if (_webSocketInfo[0] == null || _webSocketInfo[1] == null) {
					addDebugBuf(res);
				} else webSocketInfo = _webSocketInfo;
			} catch (Exception e) {
				addDebugBuf("resetWebsocketInfo exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
		}
		private string[] getQualityList(string msg) {
			var gettableList = util.getRegGroup(msg, "\"availableQualities\"\\:\\[(.+?)\\]").Replace("\"", "").Split(',');
			return gettableList;
		}
		override public void setQuality(string q) {
			addDebugBuf("set quality " + q);
			if (selectQuality != q)
				sendUseableStreamGetCommand(q);
			selectQuality = q;
			
		}
		private void sendMessage(WebSocket w, string s) {
			util.debugWriteLine("ws send " + s);
			w.Send(s);
		}
		private void rtmpRecord(string websocketMsg, string quality) {
			string url = null;
			if (websocketMsg != null) {
				var _msg = util.getRegGroup(websocketMsg,  "\"currentStream\":{(.+?)}");
				if (_msg == null) {
					util.debugWriteLine("rtmpRecord _msg null");
					return;
				}
				url = util.getRegGroup(_msg, "\"uri\":\"(.+?)\"") + "/" + util.getRegGroup(_msg, "\"name\":\"(.+?)\"");
				util.debugWriteLine("rtmp url " + url);
			}
			
			rfu.subGotNumTaskInfo = new List<numTaskInfo>();
			if (rr != null) {
				rr.resetRtmpUrl(url);
			}
			else {
				rr = new RtmpRecorder(lvid, container, rm, rfu, !isRtmp, recFolderFile, this, openTime);
				Task.Run(() => {
					rr.record(url, quality);
					rm.hlsUrl = "end";
					if (rr.isEndProgram) {
						isEndProgram = true;
						//if (endTime == DateTime.MinValue)
						//	endTime = DateTime.Now;
					}
					isRetry = false;
				});
			}
		}
		public override void setLatency(string l) {
			addDebugBuf("set latency " + l);
			if (l != null && selectLatency != l && !isRtmp) {
				var latency = float.Parse(l);
				var fid = latency % 1 == 0 ? "90" : "12";
				webSocketInfo[0] = webSocketInfo[0].Substring(0, webSocketInfo[0].Length - 2) + fid;
				webSocketInfo[1] = "{\"type\":\"startWatching\",\"data\":{\"stream\":{\"quality\":\"normal\",\"protocol\":\"hls\",\"latency\":\"" + (latency < 1.1 ? "low" : "high") + "\",\"chasePlay\":false},\"room\":{\"protocol\":\"webSocket\",\"commentable\":true},\"reconnect\":false}}";
				ws.Close();
			}
			selectLatency = l;
		}
	}
}
	