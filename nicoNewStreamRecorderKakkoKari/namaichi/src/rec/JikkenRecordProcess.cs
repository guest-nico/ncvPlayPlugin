/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/08/30
 * Time: 18:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO.Compression;
using WebSocket4Net;
//using Newtonsoft.Json;
using namaichi.info;

/*
namespace namaichi.rec
{
	/// <summary>
	/// Description of JikkenRecordProcess.
	/// </summary>
	public class JikkenRecordProcess : IRecorderProcess
	{
		//private string broadcastId;
		private string userId;
		private string lvid;
		private long releaseTime;
		private bool isPremium = false;
		private CookieContainer container;
		private string[] recFolderFile;
		public RecordingManager rm;
		private RecordFromUrl rfu;
		public JikkenRecorder jr;
		public WatchingInfo wi;
		private Record rec;
		private WebSocket wsc;
		private RtmpRecorder rr;
		private StreamWriter commentSW;
//		override public string msUri;
//		public string[] msReq;
		public long serverTime;
		private string ticket;
		private bool isRetry = true;
//		private string msThread;
//		private string sendCommentBuf = null;
//		private bool isSend184 = true;
		
		//private bool isNoPermission = false;
		//public long openTime;
		public bool isEndProgram = false;
		//public int lastSegmentNo = -1;
		//public bool isTimeShift = false;
		private TimeShiftConfig tsConfig = null;
		private bool isTimeShiftCommentGetEnd = false;
		private DateTime lastEndProgramCheckTime = DateTime.Now;
		private DateTime lastWebsocketConnectTime = DateTime.Now;
		
		public TimeSpan jisa = TimeSpan.MinValue;
		//private DateTime beginTime = null;
		//private DateTime endTime = null;
		private TimeSpan programTime = TimeSpan.Zero;
		
//		public DateTime tsHlsRequestTime = DateTime.MinValue;
//		public TimeSpan tsStartTime;
		
		//private DeflateDecoder deflateDecoder;// = new DeflateDecoder();
		CancellationTokenSource wscPongCancelToken;
		
		private bool isGetCommentXml;
		//private TimeShiftCommentGetter_jikken tscgChat;
		//private TimeShiftCommentGetter_jikken tscgControl;
		private string commentFileName = null;
		private string commentHead = null;
		
		private bool isSub;
		public bool isRtmp;
		
		public List<string> changedHlsUrl = new List<string>();
		
		public JikkenRecordProcess( 
				CookieContainer container, string[] recFolderFile, 
				RecordingManager rm, RecordFromUrl rfu, 
				JikkenRecorder jr, long openTime, 
				bool isTimeShift, string lvid, 
				TimeShiftConfig tsConfig, string userId, 
				bool isPremium, TimeSpan programTime, 
				WatchingInfo wi, long releaseTime, bool isSub, bool isRtmp)
		{
			this.container = container;
			this.recFolderFile = recFolderFile;
			this.rm = rm;
			this.rfu = rfu;
			this.jr = jr;
			this.openTime = openTime;
			this.isTimeShift = isTimeShift;
			this.lvid = lvid;
			this.tsConfig = tsConfig;
			this.userId = userId;
			this.isPremium = isPremium;
			this.programTime = programTime;
			this.wi = wi;
			this.msUri = wi.msUri;
			isGetCommentXml = bool.Parse(rm.cfg.get("IsgetcommentXml"));
			isJikken = true;
			this.releaseTime = releaseTime;
			this.isSub = isSub;
			this.isRtmp = isRtmp;
		}
		public void start() {
			util.debugWriteLine("jrp start" + util.getMainSubStr(isSub, true));
			
			process();
			//if (!isSub)
			//	displaySchedule();
			//rm.form.setStatistics(wi.visit, wi.comment);
			
			while (rm.rfu == rfu && isRetry) {
				//test
//				GC.Collect();
//				GC.WaitForPendingFinalizers();
				
				System.Threading.Thread.Sleep(1000);
				
			}
			//while (isTimeShift && !isTimeShiftCommentGetEnd && rm.rfu == rfu) 
			//	System.Threading.Thread.Sleep(300);
			
			isRetry = false;
			if (rr != null) rr.retryMode = (isEndProgram) ? 2 : 1;
			
			
			stopRecording();
			if (rec != null) 
				rec.waitForEnd();			
			
			util.debugWriteLine("closed saikai" + util.getMainSubStr(isSub, true));

		}
		private void process() {
			//util.debugWriteLine("testtest" + util.getMainSubStr(isSub, true));
			/*
			try {var a = new WebSocket4Net.MessageReceivedEventArgs(null, null).Data;}
			catch (Exception e) {
				System.Windows.Forms.MessageBox.Show("websocket", "aa");
			}
			*
			//util.debugWriteLine(System.Diagnostics.FileVersionInfo.GetVersionInfo("websocket4net.dll").
			if (isRtmp || 
				    (rm.cfg.get("IsHokan") == "true" && 
				    !rfu.isRtmpMain && !rm.isPlayOnlyMode && 
					!rfu.isSubAccountHokan && 
					rm.cfg.get("EngineMode") == "0" && !isTimeShift)) {
					rfu.subGotNumTaskInfo = new List<numTaskInfo>();
				
				rr = new RtmpRecorder(lvid, container, rm, rfu, !isRtmp, recFolderFile, this, releaseTime);
				Task.Run(() => {
					rr.record(null, null);
					rm.hlsUrl = "end";
					if (rr.isEndProgram) isEndProgram = true;
					isRetry = false;
				});
			}
			Task.Run(() => {connectKeeper();});
			if (!isRtmp)
				Task.Run(() => {record();});
			
			
		}
		private void record() {
			rec = new Record(rm, true, rfu, wi.hlsUrl, recFolderFile[1], -1, container, isTimeShift, this, lvid, tsConfig, releaseTime, null, recFolderFile[2], isSub);
			
			if (isRtmp) jr.availableQualities = new String[]{"RTMP"};
			rm.form.setQualityList(jr.availableQualities, jr.requestQuality);
			Task.Run(() => {
				rec.record(jr.requestQuality, isRtmp);
				if (rec.isEndProgram) {
					util.debugWriteLine("stop jrp recd" + util.getMainSubStr(isSub, true));
					isRetry = false;
					if (rr != null) rr.retryMode = 2;
					isEndProgram = true;
				}
		    });
		}
		
		private void connectKeeper() {
			var start = DateTime.Now;
			while (rm.rfu == rfu && isRetry) {
				Thread.Sleep(1000);
				if (DateTime.Now - start < 
				    TimeSpan.FromMilliseconds((double)wi.expireIn)) continue;
				start = DateTime.Now;
				
				try {
					var w = jr.getWatchingPut();
					w.Wait();
					if (w.Result == null) continue;
					wi.setPutWatching(w.Result);
					//rm.form.setStatistics(wi.visit, wi.comment);
					
				} catch (Exception e) {
					util.debugWriteLine("watching put exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
				}
			}
		}
		
		override public void reConnect() {
			util.debugWriteLine("reconnect" + util.getMainSubStr(isSub, true));
			changedHlsUrl.Add(wi.hlsUrl);
			
			while (true) {
				if (rfu != rm.rfu || !isRetry) return;
				
//				var w = jr.getWatchingPut();
				var w = jr.getWatching();
				w.Wait();
				if (w.Result == null || w.Result.IndexOf("master.m3u8") == -1) {
					Thread.Sleep(3000);
					
					Task.Run(() => {
						if (!isTimeShift && isEndedProgram()) {
							isRetry = false;
							if (rr != null) rr.retryMode = 2;
							isEndProgram = true;
						}
					});
					continue;
				}
				wi.setPutWatching(w.Result);
				//rm.form.setStatistics(wi.visit, wi.comment);
				break;
			}
			
			util.debugWriteLine("reconnect got watch res " + wi.hlsUrl);
			if (!isTimeShift && wi.hlsUrl.IndexOf("hlsarchive") > -1) {
				isRetry = false;
				if (rr != null) rr.retryMode = 2;
				isEndProgram = true;
				return;
			}
			
			rec.reSetHlsUrl(wi.hlsUrl, jr.requestQuality, null, isRtmp);
		}

		public void stopRecording() {
			util.debugWriteLine("stop recording" + util.getMainSubStr(isSub, true));
			try {
				if (wsc != null && wsc.State != WebSocketState.Closed && wsc.State != WebSocketState.Closing) {
					util.debugWriteLine("state close wsc " + WebSocketState.Closed + " " + wsc.State + util.getMainSubStr(isSub, true));					
					wsc.Close();
				}
			} catch (Exception e) {
				util.debugWriteLine("wsc close error" + util.getMainSubStr(isSub, true));
				util.debugWriteLine(e.Message + e.StackTrace + util.getMainSubStr(isSub, true));
			}
			try {
				if (wscPongCancelToken != null) wscPongCancelToken.Cancel();
			} catch (Exception e) {
				util.debugWriteLine("comment sw close error" + util.getMainSubStr(isSub, true));
				util.debugWriteLine(e.Message + e.StackTrace + util.getMainSubStr(isSub, true));
			}
			/*
			try {
				if (rec != null) rec.stopRecording();
			} catch (Exception e) {
				util.debugWriteLine("rec close error" + util.getMainSubStr(isSub, true));
				util.debugWriteLine(e.Message + e.StackTrace + util.getMainSubStr(isSub, true));
			}
			isRetry = false;
			*
		}
		private bool isEndedProgram() {
			var url = "http://live2.nicovideo.jp/watch/" + lvid;
			var isPass = (DateTime.Now - lastEndProgramCheckTime < TimeSpan.FromSeconds(5)); 
			if (isPass) return false;
			lastEndProgramCheckTime = DateTime.Now;
			
			var a = new System.Net.WebHeaderCollection();
			var res = util.getPageSource(url, ref a, container);
			util.debugWriteLine("isendedprogram url " + url + " res==null " + (res == null) + util.getMainSubStr(isSub, true));
//			util.debugWriteLine("isendedprogram res " + res + util.getMainSubStr(isSub, true));
			if (res == null) return false;
			var isEnd = res.IndexOf("\"content_status\":\"closed\"") != -1 ||
					res.IndexOf("<title>番組がみつかりません") != -1 ||
					res.IndexOf("番組が見つかりません</span>") != -1;
			util.debugWriteLine("is ended program " + isEnd + util.getMainSubStr(isSub, true));
			return isEnd; 
		}
		override public void setQuality(string q) {
			util.debugWriteLine("setQuality q " + q + " jr.requestQuality " + jr.requestQuality);
			if (jr.requestQuality == q) return;
			jr.requestQuality = q;
			Task.Run(() => reConnect());
		}
		override public void setLatency(string l) {
			
		}
	}
}
*/