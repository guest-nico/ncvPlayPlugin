/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2021/10/08
 * Time: 14:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Text.RegularExpressions;
using namaichi.rec;
using namaichi.utility;

namespace namaichi.info
{
	/// <summary>
	/// Description of StreamInfo.
	/// </summary>
	public class StreamInfo {
		public string url;
		public string lvid;
		public long openTime;
		
		public string type = null;
		public string data = null;
		public string res = null;
		public bool isRtmpOnlyPage = false;
		public bool isTimeShift = false;
		public bool isChasable = false;
		public bool isReservation = false;
		
		public string communityNum = null;
		
		public long endTime, _openTime, serverTime, vposBaseTime;
		public TimeSpan programTime;
		public string[] recFolderFileInfo;
		public bool isChannelPlus = false;
		
		public StreamInfo() {
		}
		public StreamInfo (string url, string lvid, bool isTimeShift, bool isChannelPlus) {
			this.url = url;
			this.lvid = lvid;
			this.isTimeShift = isTimeShift;
			this.isChannelPlus = isChannelPlus;
		}
		//public void set(string res, CookieContainer cc) {
		public void set(string res) {
			if (lvid.StartsWith("lv")) {
				type = util.getRegGroup(res, "\"content_type\":\"(.+?)\"");
				data = util.getRegGroup(res, "<script id=\"embedded-data\" data-props=\"([\\d\\D]+?)</script>");
				this.res = res; 
				isRtmpOnlyPage = res.IndexOf("%3Cgetplayerstatus%20") > -1 || res.IndexOf("<getplayerstatus ") > -1;
				
				isChasable = util.getRegGroup(res, "&quot;permissions&quot;:\\[[^\\]]*(CHASE_PLAY)") != null &&
					res.IndexOf("isChasePlayEnabled&quot;:true") > -1;
				
				data = (isRtmpOnlyPage) ? System.Web.HttpUtility.UrlDecode(res) :
							System.Web.HttpUtility.HtmlDecode(data);
				isReservation = lvid.StartsWith("lv") ? data.IndexOf("\"reservation\"") > -1 : data.IndexOf("\"has_archived_files\":true") > -1;
				
				//long endTime, _openTime, serverTime, vposBaseTime;
				//openTime = endTime = _openTime = serverTime = vposBaseTime = 0;
		    } else {
			    type = "chplus";
			    data = this.res = res;
			    isChasable = false;
			    isReservation = false;
		    }
			programTime = util.getUnixToDatetime(endTime) - util.getUnixToDatetime(openTime);
			recFolderFileInfo = getHtml5RecFolderFileInfo(data, type, isRtmpOnlyPage);
		}
		public bool getTimeInfo() {
			if (data == null) return false;
			
			if (lvid.StartsWith("lv")) {
				if (!isRtmpOnlyPage) {
					if (!long.TryParse(util.getRegGroup(data, "\"beginTime\":(\\d+)"), out openTime))
							return false;
		//				var openTime = long.Parse(util.getRegGroup(data, "\"beginTimeMs\":(\\d+)"));
					if (!long.TryParse(util.getRegGroup(data, "\"endTime\":(\\d+)"), out endTime))
							return false;				
					if (!long.TryParse(util.getRegGroup(data, "\"openTime\":(\\d+)"), out _openTime))
							return false;
					if (!long.TryParse(util.getRegGroup(data, "\"serverTime\":(\\d+)"), out serverTime))
							return false;
					if (!long.TryParse(util.getRegGroup(data, "\"vposBaseTime\":(\\d+)"), out vposBaseTime))
							return false;
				} else {
					if (!long.TryParse(util.getRegGroup(data, "<start_time>(\\d+)"), out openTime))
							return false;
		//				var openTime = long.Parse(util.getRegGroup(data, "\"beginTimeMs\":(\\d+)"));
					if (!long.TryParse(util.getRegGroup(data, "<end_time>(\\d+)"), out endTime))
							return false;				
					if (!long.TryParse(util.getRegGroup(data, "<base_time>(\\d+)"), out _openTime))
							return false;
					if (!long.TryParse(util.getRegGroup(data, "status=\"ok\" time=\"(\\d+)\""), out serverTime))
							return false;
				}
			} else {
				string startTimeDtStr = util.getRegGroup(data, "\"live_started_at\":\"(.+?)\"");
				if (startTimeDtStr == null) startTimeDtStr = util.getRegGroup(data, "\"live_scheduled_start_at\":\"(.+?)\"");
				if (startTimeDtStr == null) startTimeDtStr = util.getRegGroup(data, "\"released_at\":\"(.+?)\"");
				var endTimeDtStr = util.getRegGroup(data, "\"live_finished_at\":\"(.+?)\"");
				if (endTimeDtStr == null) endTimeDtStr = util.getRegGroup(data, "\"live_scheduled_end_at\":\"(.+?)\"");
				if (endTimeDtStr == null) endTimeDtStr = util.getRegGroup(data, "\"released_at\":\"(.+?)\"");
				if (startTimeDtStr == null || endTimeDtStr == null) return false;
				openTime = _openTime = util.getUnixTime(DateTime.Parse(startTimeDtStr) + TimeSpan.FromHours(9));
				endTime = util.getUnixTime(DateTime.Parse(endTimeDtStr) + TimeSpan.FromHours(9));
				
				serverTime = vposBaseTime = util.getUnixTime();
			}
			return true;
		}
		private string[] getHtml5RecFolderFileInfo(string data, string type, bool isRtmpOnlyPage) {
			string host, group, title, communityNum, userId;
			host = group = title = communityNum = userId = null;
			
			if (lvid.StartsWith("lv")) {
				if (!isRtmpOnlyPage) {
	//				host = group = title = communityNum = userId = null;
					if (type == "official") {
						group = util.getRegGroup(data, "\"socialGroup\".+?\"name\".\"(.+?)\"");
						
			//			if (host == null) host = "official";
						host = util.getRegGroup(data, "\"supplier\"..\"name\".\"(.*?)\"");
						if (host == null || host == "") {
							//group = "official";
							host = "公式生放送";
						}
						if (group == null || group == "") group = "official"; 
						if (util.getRegGroup(data, "(\"socialGroup\".\\{\\},)") != null) host = "公式生放送";
						title = util.getRegGroup(data, "\"title\"\\:\"(.+?)\",");
		//				title = util.uniToOriginal(title);
						communityNum = util.getRegGroup(data, "\"socialGroup\".+?\"id\".\"(.+?)\"");
						if (communityNum == null) communityNum = "official";
						userId = "official";
						
						util.debugWriteLine(host + " " + group + " " + title + " " + communityNum);
						if (host == null || group == null || title == null || communityNum == null) return null;
						util.debugWriteLine(host + " " + group + " " + title + " " + communityNum);
					} else {
						bool isAPI = false;
						if (isAPI) {
							//var a = new System.Net.WebHeaderCollection();
							//var apiRes = util.getPageSource(url + "/programinfo", container);
						
						} else {
							
							var isChannel = util.getRegGroup(data, "visualProviderType\":\"(channel)\",\"title\"") != null;
				//			host = util.getRegGroup(res, "provider......name.....(.*?)\\\\\"");
							group = util.getRegGroup(data, "\"socialGroup\".*?\"name\".\"(.*?)\"");
				//			group = util.uniToOriginal(group);
				//			group = util.getRegGroup(res, "communityInfo.\".+?title.\"..\"(.+?).\"");
							host = util.getRegGroup(data, "\"supplier\"..\"name\".\"(.*?)\"");
							if (string.IsNullOrEmpty(host)) host = group;
				//			System.out.println(group);
				//			host = util.uniToOriginal(host);
				//			title = util.getRegGroup(res, "\\\"programHeader\\\"\:\{\\\"thumbnailUrl\\\".+?\\\"title\\\"\:\\\"(.*?)\\\"");
				//			title = util.getRegGroup(res, "\\\\\"programHeader\\\\\":\\{\\\\\"thumbnailUrl.+?\\\\\"title\\\\\":\\\\\"(.*?)\\\\\"");
							title = util.getRegGroup(data, "visualProviderType\":\"(community|channel)\",\"title\":\"(.*?)\",", 2);
				//			communityNum = util.getRegGroup(res, "socialGroup: \\{[\\s\\S]*registrationUrl: \"http://com.nicovideo.jp/motion/(.*?)\\?");
							communityNum = util.getRegGroup(data, "\"socialGroup\".+?\"id\".\"(.+?)\"");
				//			community = util.getRegGroup(res, "socialGroup\\:)");
							userId = (isChannel) ? "channel" : (util.getRegGroup(data, "supplier\":{\"name\".+?pageUrl\":\"https*://www.nicovideo.jp/user/(\\d+?)\""));
							util.debugWriteLine("userid " + userId);
				
							util.debugWriteLine("title " + title);
							util.debugWriteLine("community " + communityNum);
				//			community = util.getRegGroup(res, "socialGr(oup:)");
				//			title = util.getRegGroup(res, "\\\"programHeader\\\"\\:\\{\\\"thumbnailUrl\\\".+?\\\"title\\\"\\:\\\"(.*?)\\\"");
							//  ,\"programHeader\":{\"thumbnailUrl\":\"http:\/\/icon.nimg.jp\/community\/s\/123\/co1231728.jpg?1373210036\",\"title\":\"\u56F2\u7881\",\"provider
				//			title = util.uniToOriginal(title);
							util.debugWriteLine(host + " " + group + " " + title + " " + communityNum + " userid " + userId);
							if (host == null || group == null || title == null || communityNum == null || userId == null) return null;
						}
						
					}
					
				} else {
					group = util.getRegGroup(data, "class=\"ch_name\" title=\"(.+?)\"");
					if (group == null) group = "official";
					
					host = util.getRegGroup(data, "（提供:(.+?)）");
					if (host == null) host = "公式生放送";
					title = util.getRegGroup(data, "<title>(.+?)</title>");
	//				title = util.uniToOriginal(title);
					communityNum = util.getRegGroup(data, "channel/(ch\\d+)");
					if (communityNum == null) communityNum = "official";
					userId = "official";
					
					util.debugWriteLine(host + " " + group + " " + title + " " + communityNum);
					if (host == null || group == null || title == null || communityNum == null) return null;
					util.debugWriteLine(host + " " + group + " " + title + " " + communityNum);
				}
			} else {
				communityNum = "000";
				group = host = "ニコニコチャンネルプラス"; 
				var _cn = util.getRegGroup(res, "\"fanclub_site\".+?\"id\":(\\d+)");
				if (_cn != null) {
					communityNum = _cn;
					var c = new Curl();
					var r = c.getStr("https://nfc-api.nicochannel.jp/fc/fanclub_sites/" + _cn + "/page_base_info", util.getHeader(), CurlHttpVersion.CURL_HTTP_VERSION_2TLS);
					if (r != null) {
						var _group = util.getRegGroup(r, "\"fanclub_site_name\":\"(.+?)\",\"favicon");
						if (_group != null) group = host = _group;
						_cn = util.getRegGroup(r, "\"fanclub_code\":\"(.+?)\"");
						if (_cn != null) communityNum = _cn;
					}
				}
				title = util.getRegGroup(res, "\"title\":\"(.+?)\"");
				userId = util.getRegGroup(url, "https://nicochannel.jp/(.+?)/");
				if (userId == null) userId = communityNum;
				title = title.Replace("\\", "");
				host = host.Replace("\\", "");
				group = group.Replace("\\", "");
				
				util.debugWriteLine(host + " " + group + " " + title + " " + communityNum);
				if (host == null || group == null || title == null || communityNum == null) return null;
				util.debugWriteLine(host + " " + group + " " + title + " " + communityNum);
			}
			//if (communityNum != null) communityNum = communityNum;
			return new string[]{host, group, title, lvid, communityNum, userId};

		}
		public StreamInfo clone() {
			return (StreamInfo)this.MemberwiseClone();
		}
	}
}
