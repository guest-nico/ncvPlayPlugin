﻿/*
 * Created by SharpDevelop.
 * User: user
 * Date: 2018/10/30
 * Time: 20:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using namaichi.info;
using SuperSocket.ClientEngine;

namespace namaichi.rec
{
	/// <summary>
	/// Description of RtmpRecorder.
	/// </summary>
	public class RtmpRecorder
	{
		private string getPlayerStatusRes;
		private string lvid;
		private CookieContainer container;
		private config.config cfg;
		private RecordingManager rm;
		private RecordFromUrl rfu;
		private string recFolderFile;
		private IRecorderProcess wr;
		private long openTime;
		
		private DateTime lastGetPlayerStatusAccessTime = DateTime.MinValue;
		private bool isSub;
		private bool isTimeshift;
		public int retryMode = 0;//0-retry 1-stop 2-endProgram
		
		private Process rtmpdumpP;
		private Process ffmpegP;
		
		private DateTime lastConnectTime = DateTime.MinValue;
		public bool isEndProgram = false;
		private DateTime lastEndProgramCheckTime = DateTime.Now;
		public int subNtiGroupNum = 0;
		private int afterConvertMode;
		private int tsRecordIndex = 0;
		private int tsRecordNum = 0;
		
		private string rtmpUrl;
		private string que;
		private string ticket;
		
		public RtmpRecorder(string lvid, CookieContainer container, 
				RecordingManager rm, RecordFromUrl rfu, bool isSub,
				string[] recFolderFile, IRecorderProcess wr, long openTime) {
//			this.getPlayerStatusRes = getPlayerStatusRes;
			this.lvid = lvid;
			this.container = container;
//			this.cfg = cfg;
			this.rm = rm;
			this.rfu = rfu;
			this.isSub = isSub;
			this.wr = wr;
			this.openTime = openTime;
			this.recFolderFile = recFolderFile[1];
			rm.isTitleBarInfo = bool.Parse(rm.cfg.get("IstitlebarInfo"));
			afterConvertMode = int.Parse(rm.cfg.get("afterConvertMode"));
		}
		public void record() {
			//endcode 0-その他の理由 1-stop 2-最初に終了 3-始まった後に番組終了
			util.debugWriteLine("rtmp recorder" + util.getMainSubStr(isSub, true));
			var _m = (rm.isPlayOnlyMode) ? "視聴" : "録画";
			if (wr.isTimeShift) {
				rm.form.addLogText("タイムシフトの" + _m + "を開始します");
			} else {
				if (isSub) {
					rm.form.addLogText(_m + "をスタンバイします(サブ)");
				} else
					rm.form.addLogText(_m + "を開始します(メイン)");
			}
			
			var convertList = new List<string>();
			var isFirst = true;
			while (rm.rfu == rfu && retryMode == 0) {
				if (DateTime.Now < lastConnectTime + TimeSpan.FromSeconds(3)) {
					Thread.Sleep(500);
					continue;
				}
				lastConnectTime = DateTime.Now;
				
				var rtmpdumpArg = getRtmpDumpArgs();
				if (rtmpdumpArg == "end") {
					isEndProgram = true;
					break;
				} else if (rtmpdumpArg == "no") {
					rm.form.addLogText("データが見つかりませんでした");
					return;
				}
				if (rtmpdumpArg == null) continue;
				
				if (rm.isPlayOnlyMode) {
					while(rm.rfu == rfu && retryMode == 0) {
						Thread.Sleep(1000);
					}
					return;
				}
				
				
				
				if (!rm.isPlayOnlyMode)
					getProcess(out rtmpdumpP, out ffmpegP, rtmpdumpArg);
				
				if (!isSub) {
					//if (!isFirst && !rm.isPlayOnlyMode) wr.resetCommentFile();
					isFirst = false;
					
					Task.Run(() => errorReadProcess(rtmpdumpP));
					
					while(rm.rfu == rfu && retryMode == 0 && !rtmpdumpP.HasExited) {
						if (!rm.isPlayOnlyMode && rtmpdumpP.WaitForExit(300)) break;
						if (rm.isPlayOnlyMode) Thread.Sleep(1000);
					}
					util.debugWriteLine("rtmp Process loop end");
					
					if (rm.rfu != rfu || retryMode == 1) {
						util.debugWriteLine("retrymode " + retryMode);
						stopRecording();
					} else {
						//end program
						while (rm.rfu == rfu && !rtmpdumpP.HasExited) {
							Thread.Sleep(500);
						}
					}
					try {
						var f = new FileInfo(util.getOkSJisOut(recFolderFile) + ".flv");
						if (f != null && f.Exists && f.Length == 0) 
							File.Delete(f.FullName + ".flv");
						else {
//							if (rm.cfg.get("IsAfterRenketuFFmpeg") == "true" ||
//							   		(afterConvertMode != "0" && afterConvertMode != "4")) {
							//if (afterConvertMode > 0) {
							//	convertList.Add(recFolderFile + ".flv");
							//}
							//recFolderFile = wr.getRecFilePath()[1];
						}
					} catch (Exception e) {
						util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
						
					}
					
					if (rm.rfu != rfu || retryMode != 0) break;
					
				} else {
					var rtmpdumpTask = 
							Task.Run(() => rtmpdumpReadFFmpegWriteProcess(rtmpdumpP, ffmpegP));
//					var ffmpegTask = Task.Run(() => ffmpegReadProcess(rtmpdumpP));
					
					var isContinue = false;
					while (rm.rfu == rfu && retryMode == 0 && !rtmpdumpP.HasExited) {
//						if (taskEnd(rtmpdumpTask) && taskEnd(ffmpegTask)) {
						if (taskEnd(rtmpdumpTask)) {
							isContinue = true;
						    break;
						}
//						if (taskEnd(rtmpdumpTask) || taskEnd(ffmpegTask)) {
						if (taskEnd(rtmpdumpTask)) {
//							stopRecording();
//							break;
						}
						
						
						Thread.Sleep(500);
					}
					util.debugWriteLine("rtmp rec end isContinue " + isContinue + "(サブ)");
					if (isContinue || retryMode == 0) {
						Thread.Sleep(1000);
						continue;
					}
					
					util.debugWriteLine("rtmp rec go retryMode " + retryMode + "(サブ)");
					if (rm.rfu != rfu || retryMode == 1) {
						stopRecording();
					}
					if (retryMode == 2) {
						//end program
						util.debugWriteLine("rtmp endprogram retryMode " + retryMode);
						while (rm.rfu == rfu) {
							if ((rtmpdumpP == null || rtmpdumpP.HasExited) && (ffmpegP == null || ffmpegP.HasExited)) break;
							Thread.Sleep(500);
						}
					}
					util.debugWriteLine("rtmp rec end");
					if (rm.rfu != rfu || retryMode != 0) break;
				}
			}
			
		}
		private string getRtmpDumpArgs() {
			var url = "http://live.nicovideo.jp/api/getplayerstatus/" + lvid;
			while (rm.rfu == rfu && retryMode == 0
			      ) {
				var res = util.getPageSource(url, container, null, false, 3000);
				util.debugWriteLine(res + util.getMainSubStr(isSub, true));
				if (res == null) {
					Task.Run(() => isEndedProgram());
					Thread.Sleep(3000);
					continue;
				}
				if (res.IndexOf("<code>notlogin</code>") > -1) {
					var c = new CookieGetter(rm.cfg);
					var t = c.getHtml5RecordCookie(rfu.url, isSub);
					t.Wait();
					if (t.Result != null && t.Result[0] != null)
						container = t.Result[0];
					continue;
				}
				var pageType = util.getPageTypeRtmp(res, ref isTimeshift, isSub);
				if (pageType == 1) {
					Thread.Sleep(90000);
					continue;
				}
				if (!wr.isTimeShift && (pageType == 7 || pageType == 2)) {
					retryMode = 2;
					return "end";
				}
				if (pageType == 2 || pageType == 3) {
					return "no";
				}
				if (pageType != 0 && pageType != 7) {
					Thread.Sleep(3000);
					continue;
				}
				var xml = new XmlDocument();
				xml.LoadXml(res);
				
//				util.debugWriteLine(container.GetCookieHeader(new Uri(url)));
				var type = util.getRegGroup(res, "<provider_type>(.+?)</provider_type>");
//				string rtmpurl = null, ticket = null;
				getTicketUrl(out rtmpUrl, out ticket);
				 
				var arg = getArgFromRes(xml, pageType, type, ticket, rtmpUrl);
				if (arg == null) continue;
				return arg;
			}
			
			Thread.Sleep(3000);
			return null;
		}
		private void getTicketUrl(out string url, out string ticket) {
			var edgeurl = "http://watch.live.nicovideo.jp/api/getedgestatus?v=" + lvid;
//			util.debugWriteLine(container.GetCookieHeader(new Uri(edgeurl)));
			var res = util.getPageSource(edgeurl, container, null, false, 5000);
			url = util.getRegGroup(res, "<url>(.+?)</url>");
			ticket = util.getRegGroup(res, "<ticket>(.+?)</ticket>");
//			int i = 0;
		}
		private string getArgFromRes(XmlDocument xml, int pageType, string type, string ticket, string url) {
			if (pageType == 0) {
				if (type == "official") {
					var _ticket = xml.SelectSingleNode("/getplayerstatus/tickets");
//					string ticket = null;
					if (_ticket != null) {
						var name = _ticket.ChildNodes[0].Attributes["name"];
						if (name != null) {
							string arg = null;
							if (name.Value.IndexOf("@") > -1) {
								ticket = _ticket.ChildNodes[0].InnerText;
								arg = "-vr \"rtmp://nlakmjpk.live.nicovideo.jp:1935/live/" + name.Value + "?" + ticket + "\"";
							} else {
								if (_ticket.ChildNodes.Count == 10) ticket = _ticket.ChildNodes[0].InnerText;
								else ticket = _ticket.ChildNodes[_ticket.ChildNodes.Count - 1].InnerText;
								arg = "-vr \"rtmp://smilevideo.fc.llnwd.net:1935/smilevideo/s_" + lvid + "?" + ticket + "\"";
							}
							if (ticket == null) {
								Thread.Sleep(3000);
								return null;
							}
							if (!isSub) rm.hlsUrl = arg;
							if (!isSub && !rm.isPlayOnlyMode) arg += " -o \"" + util.getOkSJisOut(recFolderFile) + ".flv\"";
							util.debugWriteLine(arg + util.getMainSubStr(isSub, true));
							return arg;
						}
					}
					Thread.Sleep(3000);
					return null;
					
				} else {
					var _contentsUrl = xml.SelectSingleNode("/getplayerstatus/stream/contents_list/contents");
					var contentsUrl = (_contentsUrl == null) ? null : _contentsUrl.InnerText.Substring(5);
					/*
					var _rtmpUrl = xml.SelectSingleNode("/getplayerstatus/rtmp/url");
					var rtmpUrl = (_rtmpUrl == null) ? null : _rtmpUrl.InnerText;
					var _ticket = xml.SelectSingleNode("/getplayerstatus/rtmp/ticket");
					var ticket = (_ticket == null) ? null : _ticket.InnerText;
					*/
					util.debugWriteLine(type + " contentsUrl " + contentsUrl + " rtmpUrl " + rtmpUrl + " ticket " + ticket + util.getMainSubStr(isSub, true));
					var arg = "-vr " + rtmpUrl + "/" + lvid + " -N " + contentsUrl + " -C S:" + ticket;
					if (!isSub) rm.hlsUrl = arg;
					
					if (!isSub && !rm.isPlayOnlyMode) arg += " -o \"" + util.getOkSJisOut(recFolderFile) + ".flv\"";
					util.debugWriteLine(arg + util.getMainSubStr(isSub, true));
					if (contentsUrl == null || rtmpUrl == null || ticket == null) {
						Thread.Sleep(3000);
						return null;
					}
					//rtmpdump エラー
					//derKakkoKari\namaichi\bin\Debug\rec/武田庵路/武田庵路_co2760796(武田食堂)_lv316893954(【世界名作RPG劇場】LIVE A ﾖVI˩ 最終章 中世編【Part8】)_1.flv"(
					//˩˩˩˩˩˩˩˩
					//˩
					return arg;
				}
			} else {
				//timeshift
				var isPremium = xml.SelectSingleNode("/getplayerstatus/user/is_premium").InnerText == "1";
				if (type == "official" || type == "channel") {
					/*
					var _url = xml.SelectSingleNode("/getplayerstatus/rtmp/url");
					rtmpUrl = (_url == null) ? null : _url.InnerText;
					var _ticket = xml.SelectSingleNode("/getplayerstatus/rtmp/ticket");
					ticket = (_ticket == null) ? null : _ticket.InnerText;
					*/
					var que = xml.SelectSingleNode("/getplayerstatus/stream/quesheet");
					if (rtmpUrl == null || ticket == null || que == null) {
						Thread.Sleep(3000);
						return null;
					}
					
					var play = getPlay(que, isPremium);
					if (play == null) {
						Thread.Sleep(3000);
						return null;
					}
					var publishList = getPublishList(que, play);
					if (publishList.Count == 0) return "no";
					
					var arg = "";
					foreach (var a in publishList) {
//						string _out = (arg == "") ? util.getOkSJisOut(recFolderFile[1]) : wr.getRecFilePath(0)[1];
						if (arg != "") arg += "$";
						var app = util.getRegGroup(rtmpUrl, "(fileorigin.+)");
						arg += "-r " + rtmpUrl + " -y mp4:" + a + " -a " + app + " -p http://live.nicovideo.jp/watch/" + lvid + " -s \"http://live.nicovideo.jp/nicoliveplayer.swf\" -f \"WIN 29,0,0,113\" -t " + rtmpUrl + " -C S:" + ticket + " -o ";
						//arg += "-r " + url + " -y mp4:" + a + " -C S:" + ticket + " -o ";
					}
					rm.hlsUrl = "timeshift";
					util.debugWriteLine(arg + util.getMainSubStr(isSub, true));
					return arg;
					
				} else {
					/*
					var _url = xml.SelectSingleNode("/getplayerstatus/rtmp/url");
					var __url = (_url == null) ? null : _url.InnerText;
					var _ticket = xml.SelectSingleNode("/getplayerstatus/rtmp/ticket");
					var __ticket = (_ticket == null) ? null : _ticket.InnerText;
					*/
					var que = xml.SelectSingleNode("/getplayerstatus/stream/quesheet");
					if (rtmpUrl == null || ticket == null || que == null) {
						Thread.Sleep(3000);
						return null;
					}
					
					var play = getPlay(que, isPremium);
					if (play == null) {
						Thread.Sleep(3000);
						return null;
					}
					var publishList = getPublishList(que, play);
					if (publishList.Count == 0) return "no";
					
					var arg = "";
					foreach (var a in publishList) {
						if (arg != "") arg += "$";
						arg += "-vr " + rtmpUrl + " -N " + a + " -C S:" + ticket + " -p http://live.nicovideo.jp/watch/" + lvid + " -s http://live.nicovideo.jp/nicoliveplayer.swf?180116154229 -f \"WIN 29,0,0,113\" " + " -o ";
					}
					rm.hlsUrl = "timeshift";
					util.debugWriteLine(arg + util.getMainSubStr(isSub, true));
					return arg;
					
				}
			}
		}
		private string getPlay(XmlNode ques, bool isPremium) {
			string defaultP = null;
			string premiumP = null;
			foreach (XmlNode q in ques.ChildNodes) {
				var qi = q.InnerText;
				if (qi.StartsWith("/play case")) {
					if (qi.IndexOf("default:rtmp:") > -1)
						defaultP = util.getRegGroup(qi, "default:rtmp:(.+?)[, $]");
					if (qi.IndexOf("premium:rtmp:") > -1)
						premiumP = util.getRegGroup(qi, "premium:rtmp:(.+?)[, $]");
				} else 
					if (qi.StartsWith("/play")) return util.getRegGroup(qi, "rtmp:(.+?)[, $]");
			}
			if (premiumP != null) return premiumP;
			else if (defaultP != null) return defaultP;
			return null;
		}
		private List<string> getPublishList(XmlNode que, string play) {
			var l = new List<string>();
			foreach (XmlNode q in que.ChildNodes) {
				var qi = q.InnerText;
				if (qi.StartsWith("/publish " + play))
					l.Add(util.getRegGroup(qi, "/publish " + play + " (.+)"));
			}
			return l;
		}
		/*
		public int getPageType(string res) {
//			var res = getPlayerStatusRes;
			if (res.IndexOf("status=\"ok\"") > -1 && res.IndexOf("<archive>0</archive>") > -1) {
				isTimeshift = false;
				return 0;
			}
			if (res.IndexOf("status=\"ok\"") > -1 && res.IndexOf("<archive>1</archive>") > -1) {
				isTimeshift = true;
				return 7;
			}
			else if (res.IndexOf("<code>require_community_member</code>") > -1) return 4;
			else if (res.IndexOf("<code>closed</code>") > -1) return 2;
			else if (res.IndexOf("<code>comingsoon</code>") > -1) return 5;
			else if (res.IndexOf("<code>notfound</code>") > -1) return 2;
			else if (res.IndexOf("<code>deletedbyuser</code>") > -1) return 2;
			else if (res.IndexOf("<code>deletedbyvisor</code>") > -1) return 2;
			else if (res.IndexOf("<code>violated</code>") > -1) return 2;
			else if (res.IndexOf("<code>usertimeshift</code>") > -1) return 2;
			else if (res.IndexOf("<code>tsarchive</code>") > -1) return 2;
			else if (res.IndexOf("<code>unknown_error</code>") > -1) return 5;
			else if (res.IndexOf("<code>timeshift_ticket_exhaust</code>") > -1) return 2;
			else if (res.IndexOf("<code>timeshiftfull</code>") > -1) return 1;
			else if (res.IndexOf("<code>maintenance</code>") > -1) return 5;
			else if (res.IndexOf("<code>noauth</code>") > -1) return 5;
			else if (res.IndexOf("<code>full</code>") > -1) return 1;
			else if (res.IndexOf("<code>block_now_count_overflow</code>") > -1) return 5;
			else if (res.IndexOf("<code>premium_only</code>") > -1) return 5;
			else if (res.IndexOf("<code>selected-country</code>") > -1) return 5;
			else if (res.IndexOf("<code>notlogin</code>") > -1) return 8;
			rm.form.addLogText(res + util.getMainSubStr(isSub, true));
			return 5;
		}
		*/
		private string[] getRecFolderFileInfo(string res, string type) {
			/*
			string host, group, title, communityNum, userId;
			host = group = title = communityNum = userId = null;
			if (type == "official") {
//				host = util.getRegGroup(data, "\"socialGroup\".+?\"name\".\"(.+?)\"");
//				if (util.getRegGroup(data, "(\"socialGroup\".\\{\\},)") != null) host = "公式生放送";
				host = "公式生放送";
	//			if (host == null) host = "official";
				//group = util.getRegGroup(data, "\"supplier\"..\"name\".\"(.+?)\"");
//				group = "official";
				if (group == null) group = "official";
				title = util.getRegGroup(data, "<title>(.+?)</title>");
//				title = util.uniToOriginal(title);
//				communityNum = util.getRegGroup(data, "\"socialGroup\".+?\"id\".\"(.+?)\"");
				if (communityNum == null) communityNum = "official";
				userId = "official";
				
				util.debugWriteLine(host + " " + group + " " + title + " " + communityNum + util.getMainSubStr(isSub, true));
				if (host == null || group == null || title == null || communityNum == null) return null;
				util.debugWriteLine(host + " " + group + " " + title + " " + communityNum + util.getMainSubStr(isSub, true));
			} else {
				bool isAPI = false;
				if (isAPI) {
//					var a = new System.Net.WebHeaderCollection();
//					var apiRes = util.getPageSource(url + "/programinfo", ref a, container);
				
				} else {
					var isCommunity = util.getRegGroup(data, "providerType\":\"(community)\",\"title\"") != null;
		//			host = util.getRegGroup(res, "provider......name.....(.*?)\\\\\"");
					//group = util.getRegGroup(data, "\"community\".+?\"name\".\"(.+?)\"");
		//			group = util.uniToOriginal(group);
		//			group = util.getRegGroup(res, "communityInfo.\".+?title.\"..\"(.+?).\"");
					host = util.getRegGroup(data, "<owner_name>(.+?)</owner_name>");
		//			System.out.println(group);
		//			host = util.uniToOriginal(host);
		//			title = util.getRegGroup(res, "\\\"programHeader\\\"\:\{\\\"thumbnailUrl\\\".+?\\\"title\\\"\:\\\"(.*?)\\\"");
		//			title = util.getRegGroup(res, "\\\\\"programHeader\\\\\":\\{\\\\\"thumbnailUrl.+?\\\\\"title\\\\\":\\\\\"(.*?)\\\\\"");
					title = util.getRegGroup(data, "<title>(.+?)</title>");
		//			communityNum = util.getRegGroup(res, "socialGroup: \\{[\\s\\S]*registrationUrl: \"http://com.nicovideo.jp/motion/(.*?)\\?");
					communityNum = util.getRegGroup(data, "<default_community>(.+?)</default_community>");
		//			community = util.getRegGroup(res, "socialGroup\\:)");
					group = getCommunityName(communityNum);
					userId = util.getRegGroup(data, "\"broadcaster\"..\"id\".\"(.+?)\"");
					//userId = (isChannel) ? "channel" : (util.getRegGroup(data, "supplier\":{\"name\".+?pageUrl\":\"http://www.nicovideo.jp/user/(\\d+?)\""));
					util.debugWriteLine("userid " + userId + util.getMainSubStr(isSub, true));
		
					util.debugWriteLine("title " + title + util.getMainSubStr(isSub, true));
					util.debugWriteLine("community " + communityNum + util.getMainSubStr(isSub, true));
		//			community = util.getRegGroup(res, "socialGr(oup:)");
		//			title = util.getRegGroup(res, "\\\"programHeader\\\"\\:\\{\\\"thumbnailUrl\\\".+?\\\"title\\\"\\:\\\"(.*?)\\\"");
					//  ,\"programHeader\":{\"thumbnailUrl\":\"http:\/\/icon.nimg.jp\/community\/s\/123\/co1231728.jpg?1373210036\",\"title\":\"\u56F2\u7881\",\"provider
		//			title = util.uniToOriginal(title);
					
					util.debugWriteLine(host + " " + group + " " + title + " " + communityNum + " userid " + userId + util.getMainSubStr(isSub, true));
					if (host == null || group == null || title == null || communityNum == null || userId == null) return null;
				}
			}
			if (communityNum != null) rm.communityNum = communityNum;
			return new string[]{host, group, title, lvid, communityNum, userId};
			*/
			return null;
		}
		private void h5rTekina() {
			
			/*
			recFolderFileInfo = null;
			string[] recFolderFile = null;
			var type = util.getRegGroup(res, "<provider_type>(.+?)</provider_type>");
			
			long openTime = 0;
			if (data == null || 
			    !long.TryParse(util.getRegGroup(data, "<start_time>(\\d+)</start_time>"), out openTime))
					return 3;
//				var openTime = long.Parse(util.getRegGroup(data, "\"beginTimeMs\":(\\d+)"));
			openTime = openTime;
			long endTime = 0;
			if (data == null || 
			    !long.TryParse(util.getRegGroup(data, "<end_time>(\\d+)</end_time>"), out endTime))
					return 3;				
			endTime = endTime;
			var programTime = util.getUnixToDatetime(endTime) - util.getUnixToDatetime(openTime);
			long releaseTime = 0;
			if (data == null || 
			    !long.TryParse(util.getRegGroup(data, "<start_time>(\\d+)</start_time>"), out releaseTime))
					return 3;				
			releaseTime = releaseTime;
			
			recFolderFileInfo = getRecFolderFileInfo(res, type);
			if (!isSub) {
				timeShiftConfig = null;
				if (!isLive) {
					if (rm.ri != null) timeShiftConfig = rm.ri.tsConfig;
					if (rm.argTsConfig != null) {
						timeShiftConfig = getReadyArgTsConfig(rm.argTsConfig, recFolderFileInfo[0], recFolderFileInfo[1], recFolderFileInfo[2], recFolderFileInfo[3], recFolderFileInfo[4], recFolderFileInfo[5], releaseTime);
					} else {
						timeShiftConfig = getTimeShiftConfig(recFolderFileInfo[0], recFolderFileInfo[1], recFolderFileInfo[2], recFolderFileInfo[3], recFolderFileInfo[4], recFolderFileInfo[5], rm.cfg, releaseTime);
						if (timeShiftConfig == null) return 2;
						
					}
				}
				
				if (!rm.isPlayOnlyMode) {
		//			util.debugWriteLine("rm.rfu " + rm.rfu.GetHashCode() + " rfu " + rfu.GetHashCode() + util.getMainSubStr(isSub, true));
		//			if (recFolderFile == null)
						recFolderFile = getRecFilePath(releaseTime);
					if (recFolderFile == null || recFolderFile[0] == null) {
						//パスが長すぎ
						rm.form.addLogText("パスに問題があります。 " + recFolderFile[1]);
						util.debugWriteLine("too long path? " + recFolderFile[1] + util.getMainSubStr(isSub, true));
						return 2;
					}
				} else recFolderFile = new string[]{"", "", ""};
					
				//display set
				var b = new RecordStateSetter(rm.form, rm, rfu, !isLive, true, recFolderFile, rm.isPlayOnlyMode);
				Task.Run(() => {
					b.set(data, type, recFolderFileInfo);
				});
				
				//hosoInfo
				if (rm.cfg.get("IshosoInfo") == "true" && !rm.isPlayOnlyMode)
					Task.Run(() => {b.writeHosoInfo();});
					
				
				util.debugWriteLine("form disposed" + rm.form.IsDisposed + util.getMainSubStr(isSub, true));
				util.debugWriteLine("recfolderfile test " + recFolderFileInfo + util.getMainSubStr(isSub, true));
				
				var fileName = System.IO.Path.GetFileName(recFolderFile[1]);
				rm.form.setTitle(fileName);
			} else {
				recFolderFile = new string[]{"", "", ""};
			}
			*/
		}
		private void getProcess(out Process rtmpdumpP, out Process ffmpegP, string rtmpdumpArg) {
			rtmpdumpP = ffmpegP = null;
			rtmpdumpP = new Process();
			var si = new ProcessStartInfo();
			si.FileName = "rtmpdump.exe";
			si.Arguments = rtmpdumpArg;
			si.UseShellExecute = false;
			si.CreateNoWindow = true;
			util.debugWriteLine("rtmp get process " + util.getMainSubStr(isSub, true));
			if (isSub) 
				si.RedirectStandardOutput = true;
			else si.RedirectStandardError = true;
			rtmpdumpP.StartInfo = si;
			rtmpdumpP.Start();
			
			if (isSub && false) {
				ffmpegP = new Process();
				var ffmpegSi = new ProcessStartInfo();
				ffmpegSi.FileName = "ffmpeg.exe";
				var ffmpegArg = "-i - -c mpegts -y pipe:1.ts";
				ffmpegSi.Arguments = ffmpegArg;
				ffmpegSi.RedirectStandardInput = true;
				ffmpegSi.RedirectStandardOutput = true;
				ffmpegSi.UseShellExecute = false;
				ffmpegSi.CreateNoWindow = true;
				ffmpegP.StartInfo = ffmpegSi;
				ffmpegP.Start();
			}
			EventHandler e = new EventHandler(appExitHandler);
			Application.ApplicationExit += e;
		}
		private void rtmpdumpReadFFmpegWriteProcess(
				Process rtmpdumpP, Process ffmpegP) {
			util.debugWriteLine("rtmpdumpReadFFmpegWriteProcess start");
			Stream o;
			try {
				Thread.Sleep(500);
				o = rtmpdumpP.StandardOutput.BaseStream;
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
				return;
			}
//			var _is = ffmpegP.StandardInput.BaseStream;
			var b = new byte[100000000];
			while (!rtmpdumpP.HasExited && rfu == rm.rfu) {
				try {
					var i = o.Read(b, 0, b.Length);
//					if (isFirst) 
//					Debug.WriteLine("rtmpdump " + i);
//					if (rm.isPlayOnlyMode) continue;
					
					if (i == null || i == 0) {
						util.debugWriteLine("rtmpdump read i " + ((i == null) ? "null" : " get 0"));
					}
					
					var bb = b.CloneRange(0, i);
					if (rfu.firstFlvData == null && bb.Length > 0) {
						rfu.firstFlvData = bb;
						util.debugWriteLine("rtmp set firstData len " + bb.Length);
					}
					var nti = new numTaskInfo(subNtiGroupNum, null, 1, null, 0, 0);
					nti.res = bb;
					rfu.subGotNumTaskInfo.Add(nti);
					
				} catch (Exception ee) {
					Debug.WriteLine("rtmpdump read exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				}
			}
			subNtiGroupNum++;
			util.debugWriteLine("rtmpdumpReadFFmpegWriteProcess end");
		}
		private void ffmpegReadProcess(Process ffmpegP) {
			util.debugWriteLine("ffmpegReadProcess start");
			Stream _os;
			try {
				_os = ffmpegP.StandardOutput.BaseStream;
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
				return;
			}
			
			var resBuf = new List<byte>();
			var b = new byte[100000000];
			while (!ffmpegP.HasExited) {
				try {
					
					var i = _os.Read(b, 0, b.Length);
//					if (isFirst) 
//					Debug.WriteLine("ff " + i);
					var bb = b.CloneRange(0, i);

					var nti = new numTaskInfo(0, null, 1, null, 0, 0);
					nti.res = bb;
//					util.debugWriteLine("ffmpeg " + nti.res.Length + util.getMainSubStr(isSub, true));
					
//					if (DateTime.Now - nti.dt > TimeSpan.FromSeconds(2)) {
						rfu.subGotNumTaskInfo.Add(nti);
//					} 
						
					
				} catch (Exception ee) {
					Debug.WriteLine(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				}
			}
			util.debugWriteLine("ffmpegReadProcess end");
		}
		private void errorReadProcess(Process p) {
			Debug.WriteLine("error read start");
			var isRecordLive = p.StartInfo.Arguments.StartsWith("-vr");
			
			StreamReader _os;
			try {
				_os = p.StandardError;
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
				return;
			}
			while (!p.HasExited) {
				try {
					var i = _os.ReadLine();
					if (i == null) break;
//					if (i.Length == 0) break;
					var isState = i.Length > 3 && 
							(i.Substring(i.Length - 2) == "%)" ||
							i.Substring(i.Length - 3) == "sec");
					if (isState) {
						//rm.form.setRecordState(i + ((isRecordLive) ? "" : ("(" + (tsRecordIndex + 1) + "/" + tsRecordNum + ")")));
					} else {
//						rm.form.addLogText(i);
					}
					
					//ts nasi
//					if (i.IndexOf("Starting download at: 0.000 kB") > -1) 
//						
				} catch (Exception ee) {
					Debug.WriteLine("error read exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
				}
			}
			Debug.WriteLine("error read end");
		}
		
		private bool taskEnd(Task t) {
			if (t == null) return true;
			return (t.IsCanceled ||
				t.IsCompleted || t.IsFaulted);
		}
		private void stopRecording() {
			util.debugWriteLine("rtmp rec stop recording" + util.getMainSubStr(isSub, true));
			_stopRecording(rtmpdumpP);
			_stopRecording(ffmpegP);
		}
		private void _stopRecording(Process p) {
			try {
				util.debugWriteLine("stop recording rtmp p " + p + " p.hasexited= " + ((p == null) ? "" : p.HasExited.ToString()));
				
				if (p == null || p.HasExited) return;
				try {
					p.Kill();
				} catch (Exception eee) {
					util.debugWriteLine(eee.Message + eee.StackTrace + eee.Source + eee.TargetSite + util.getMainSubStr(isSub, true));
				}
			
				while(!p.HasExited) {
					System.Threading.Thread.Sleep(200);
				}
				util.debugWriteLine("destroy " + p.ExitCode + util.getMainSubStr(isSub, true));
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
		}
		private void appExitHandler(object sender, EventArgs e) {
			stopRecording();
		}
		private bool isEndedProgram() {
			var isPass = (DateTime.Now - lastEndProgramCheckTime < TimeSpan.FromSeconds(5)); 
			if (isPass) return false;
			lastEndProgramCheckTime = DateTime.Now;
			
			isEndProgram = util.isEndedProgram(lvid, container, isSub);
			if (isEndProgram) retryMode = 2;
			return isEndProgram;
		}

	}
}
