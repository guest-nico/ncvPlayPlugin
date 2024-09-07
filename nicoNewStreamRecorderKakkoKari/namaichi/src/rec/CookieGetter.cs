/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/05/13
 * Time: 4:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using namaichi.gui;
using namaichi.utility;
using SunokoLibrary.Application;
using System.Net.Http;
using System.Collections.Generic;

namespace namaichi.rec
{
	/// <summary>
	/// Description of CookieGetter.
	/// </summary>
	public class CookieGetter
	{
		//private CookieContainer cc;
		public string pageSource = null;
		public bool isHtml5 = false;
		private config.config cfg;
		public string log = "";
		public string id = null;
		static readonly Uri TargetUrl = new Uri("https://live.nicovideo.jp/");
		static readonly Uri TargetUrl2 = new Uri("https://live2.nicovideo.jp");
		static readonly Uri TargetUrl3 = new Uri("https://com.nicovideo.jp");
		static readonly Uri TargetUrl4 = new Uri("https://watch.live.nicovideo.jp/api/");
		private bool isSub;
		private bool isRtmp = false;
		
		public CookieGetter(config.config cfg)
		{
			this.cfg = cfg;
		}
		async public Task<CookieContainer[]> getHtml5RecordCookie(string url, bool isSub) {
			this.isSub = isSub;
			
			CookieContainer cc;
			if (!isSub) {
				cc = await getCookieContainer(cfg.get("BrowserNum"),
						cfg.get("issecondlogin"), cfg.get("accountId"), 
						cfg.get("accountPass"), cfg.get("user_session"),
						cfg.get("user_session_secure"), false, 
						url);
				if (cc != null) {
					var c = cc.GetCookies(TargetUrl)["user_session"];
					var secureC = cc.GetCookies(TargetUrl)["user_session_secure"];
					
					var l = new List<KeyValuePair<string, string>>();
					if (c != null)
//						cfg.set("user_session", c.Value);
						l.Add(new KeyValuePair<string, string>("user_session", c.Value));
					if (secureC != null)
//						cfg.set("user_session_secure", secureC.Value);
						l.Add(new KeyValuePair<string, string>("user_session_secure", secureC.Value));
					cfg.set(l);
				}
				
			} else {
				cc = await getCookieContainer(cfg.get("BrowserNum2"),
						cfg.get("issecondlogin2"), cfg.get("accountId2"), 
						cfg.get("accountPass2"), cfg.get("user_session2"),
						cfg.get("user_session_secure2"), true, 
						url);
				if (cc != null) {
					var c = cc.GetCookies(TargetUrl)["user_session2"];
					var secureC = cc.GetCookies(TargetUrl)["user_session_secure2"];
					
					var l = new List<KeyValuePair<string, string>>();
					if (c != null)
//						cfg.set("user_session2", c.Value);
						l.Add(new KeyValuePair<string, string>("user_session2", c.Value));
					if (secureC != null)
//						cfg.set("user_session_secure2", secureC.Value);
						l.Add(new KeyValuePair<string, string>("user_session_secure2", secureC.Value));
					cfg.set(l);
				}
			}
			
			var ret = new CookieContainer[]{cc};
			return ret;
		}
		async private Task<CookieContainer> getCookieContainer(
				string browserNum, string isSecondLogin, string accountId,
				string accountPass, string userSession, string userSessionSecure,
				bool isSub, string url) {
			
			var userSessionCC = getUserSessionCC(userSession, userSessionSecure);
			log += (userSessionCC == null) ? "前回のユーザーセッションが見つかりませんでした。" : "前回のユーザーセッションが見つかりました。";
			if (userSessionCC != null && true) {
//				util.debugWriteLine(userSessionCC.GetCookieHeader(TargetUrl));
				util.debugWriteLine("usersessioncc ishtml5login" + util.getMainSubStr(isSub));
				if (isHtml5Login(userSessionCC, url)) {
					/*
					var c = userSessionCC.GetCookies(TargetUrl)["user_session"];
					var secureC = userSessionCC.GetCookies(TargetUrl)["user_session_secure"];
					if (c != null)
						//cfg.set("user_session", c.Value);
						us = c.Value;
					if (secureC != null)
						//cfg.set("user_session_secure", secureC.Value);
						uss = secureC.Value;
					*/
					return userSessionCC;
				}
			}
			
			if (browserNum == "2") {
				CookieContainer cc = await getBrowserCookie(isSub).ConfigureAwait(false);
				log += (cc == null) ? "ブラウザからユーザーセッションを取得できませんでした。" : "ブラウザからユーザーセッションを取得しました。";
				if (cc != null) {
					util.debugWriteLine("browser ishtml5login" + util.getMainSubStr(isSub));
					if (isHtml5Login(cc, url)) {
//						util.debugWriteLine("browser 1 " + cc.GetCookieHeader(TargetUrl));
//						util.debugWriteLine("browser 2 " + cc.GetCookieHeader(new Uri("http://live2.nicovideo.jp")));
						util.debugWriteLine("browser login ok" + util.getMainSubStr(isSub));
						/*
						var c = cc.GetCookies(TargetUrl)["user_session"];
						var secureC = cc.GetCookies(TargetUrl)["user_session_secure"];
						if (c != null)
							//cfg.set("user_session", c.Value);
							us = c.Value;
						if (secureC != null)
							//cfg.set("user_session_secure", secureC.Value);
							uss = secureC.Value;
						*/
						return cc;
					}
					
				}
			}
			
			if (browserNum == "1" || 
			    isSecondLogin == "true") {
				var mail = accountId;
				var pass = accountPass;
				var accCC = await getAccountCookie(mail, pass).ConfigureAwait(false);
				log += (accCC == null) ? "アカウントログインからユーザーセッションを取得できませんでした。" : "アカウントログインからユーザーセッションを取得しました。";
				if (accCC != null) {
					util.debugWriteLine("account ishtml5login" + util.getMainSubStr(isSub));
					if (isHtml5Login(accCC, url)) {
						util.debugWriteLine("account login ok" + util.getMainSubStr(isSub));
						/*
						var c = accCC.GetCookies(TargetUrl)["user_session"];
						var secureC = accCC.GetCookies(TargetUrl)["user_session_secure"];
						if (c != null)
							//cfg.set("user_session", c.Value);
							us = c.Value;
						if (secureC != null)
							//cfg.set("user_session_secure", secureC.Value);
							uss = secureC.Value;
						*/
						return accCC;
					}
				}
			}
			return null;
		}
		private CookieContainer getUserSessionCC(string us, string uss) {
			//var us = cfg.get("user_session");
			//var uss = cfg.get("user_session_secure");
			if ((us == null || us.Length == 0) &&
			    (uss == null || uss.Length == 0)) return null;
			var cc = new CookieContainer();
			
			var c = new Cookie("user_session", us);
			var secureC = new Cookie("user_session_secure", uss);
			cc = copyUserSession(cc, c, secureC);
 			cc.Add(TargetUrl, new Cookie("player_version", "leo"));
			
			//test
//			cc.Add(TargetUrl, new Cookie("nicosid", "1527623077.1259703149"));
//			cc.Add(TargetUrl, new Cookie("_td", "9278c72a-9d4e-4b77-ac40-73f972913d26"));
//			cc.Add(TargetUrl, new Cookie("_gid", "GA1.2.266519775.1527623073"));
//			cc.Add(TargetUrl, new Cookie("_ga", "GA1.2.1892636543.1527623073"));
//			cc.SetCookies(TargetUrl,"optimizelyBuckets=%7B%7D; optimizelySegments=%7B%223152721399%22%3A%22search%22%2C%223155720808%22%3A%22gc%22%2C%223199620088%22%3A%22false%22%2C%223214930722%22%3A%22false%22%2C%223218750517%22%3A%22referral%22%2C%223219110468%22%3A%22none%22%2C%223233940089%22%3A%22gc%22%2C%223235780522%22%3A%22none%22%2C%225140350011%22%3A%22%25E3%2583%25AD%25E3%2582%25B0%25E3%2582%25A4%25E3%2583%25B3%25E6%25B8%2588%22%2C%225130920861%22%3A%22%25E4%25B8%2580%25E8%2588%25AC%25E4%25BC%259A%25E5%2593%25A1%22%2C%225137970544%22%3A%22216pt%25E6%259C%25AA%25E6%25BA%2580%22%2C%229019961413%22%3A%22%25E9%259D%259E%25E5%25AF%25BE%25E8%25B1%25A1%22%7D; nicorepo_filter=all;  optimizelyEndUserId=oeu1527671506390r0.4517391591303288; " +
//			cc.Add(c);
//			cc.Add(TargetUrl, c);
			return cc;
		}
		async private Task<CookieContainer> getBrowserCookie(bool isSub) {
			var si = SourceInfoSerialize.load(isSub);
			
//			var importer = await SunokoLibrary.Application.CookieGetters.Default.GetInstanceAsync(si, false);
			ICookieImporter importer = await SunokoLibrary.Application.CookieGetters.Default.GetInstanceAsync(si, false).ConfigureAwait(false);
//			var importers = new SunokoLibrary.Application.CookieGetters(true, null);
//			var importera = (await SunokoLibrary.Application.CookieGetters.Browsers.IEProtected.GetCookiesAsync(TargetUrl));
//			foreach (var rr in importer.Cookies)
//				util.debugWriteLine(rr);
			//importer = await importers.GetInstanceAsync(si, true);
			if (importer == null) return null;

			CookieImportResult result = await importer.GetCookiesAsync(TargetUrl).ConfigureAwait(false);
			if (result.Status != CookieImportState.Success) return null;
			
			//if (result.Cookies["user_session"] == null) return null;
			//var cookie = result.Cookies["user_session"].Value;

			//util.debugWriteLine("usersession " + cookie);
			
			var cc = new CookieContainer();
			cc.PerDomainCapacity = 200;
			foreach(Cookie _c in result.Cookies) {
				try {
					cc.Add(_c);
				} catch (Exception e) {
					util.debugWriteLine("cookie add browser " + _c.ToString() + e.Message + e.Source + e.StackTrace + e.TargetSite + util.getMainSubStr(isSub));
				}
			}
//			result.AddTo(cc);
			
			var c = cc.GetCookies(TargetUrl)["user_session"];
			var secureC = cc.GetCookies(TargetUrl)["user_session_secure"];
			cc = copyUserSession(cc, c, secureC);
			
			
			return cc;
			
		}
		private bool isHtml5Login(CookieContainer cc, string url) {
			util.debugWriteLine("cookie header " + cc.GetCookieHeader(new Uri(url)));
			for (var i = 0; i < 10; i++) {
				var headers = new WebHeaderCollection();
				try {
					util.debugWriteLine("ishtml5login getpage " + url + util.getMainSubStr(isSub));
					
					var _url = (isRtmp) ? ("https://live.nicovideo.jp/api/getplayerstatus/" + util.getRegGroup(url, "(lv\\d+)")) : url;
					//pageSource = util.getPageSource(_url, ref headers, cc);
					var h = util.getHeader(cc, null, _url);
					pageSource = new Curl().getStr(_url, h, CurlHttpVersion.CURL_HTTP_VERSION_2TLS, "GET", null, false);
//					util.debugWriteLine(cc.GetCookieHeader(new Uri(_url)));
					util.debugWriteLine("ishtml5login getpage ok" + util.getMainSubStr(isSub));
				} catch (Exception e) {
					util.debugWriteLine("cookiegetter ishtml5login " + e.Message+e.StackTrace + util.getMainSubStr(isSub));
					pageSource = "";
					log += "ページの取得中にエラーが発生しました。" + e.Message + e.Source + e.TargetSite + e.StackTrace;
					continue;
				}
	//			isHtml5 = (headers.Get("Location") == null) ? false : true;
				if (pageSource == null) {
					log += "ページが取得できませんでした。";
					util.debugWriteLine("not get page" + util.getMainSubStr(isSub));
					continue;
				}
				var isLogin = !(pageSource.IndexOf("\"login_status\":\"login\"") < 0 &&
				   	pageSource.IndexOf("login_status = 'login'") < 0);
				if (isRtmp) isLogin = pageSource.IndexOf("<code>notlogin</code>") == -1;
				util.debugWriteLine("islogin " + isLogin + util.getMainSubStr(isSub));
				log += (isLogin) ? "ログインに成功しました。" : "ログインに失敗しました";
	//			if (!isLogin) log += pageSource;
				if (isLogin) {
	//				id = (isRtmp) ? util.getRegGroup(pageSource, "<user_id>(\\d+)</user_id>")
	//					: util.getRegGroup(pageSource, "\"user_id\":(\\d+)");
					id = util.getRegGroup(pageSource, "\"user_id\":(\\d+)");
					if (id == null) id = util.getRegGroup(pageSource, "user_id = (\\d+)");
					util.debugWriteLine("id " + id);
				} else {
					util.debugWriteLine("not login " + pageSource.Substring(0, 1000) + util.getMainSubStr(isSub));
				}
				return isLogin;
			}
			return false;
		}
		async public Task<CookieContainer> getAccountCookie(string mail, string pass) {
			
			if (mail == null || pass == null) {
				log += ((mail == null) ? "not set mail." : "") + ((pass == null) ? "not set mail." : "");
				return null;
			}
			
			var isNew = true;
			
			string loginUrl;
			Dictionary<string, string> param;
			if (isNew) {
				loginUrl = "https://account.nicovideo.jp/login/redirector?show_button_twitter=1&site=niconico&show_button_facebook=1&sec=header_pc&next_url=/";
				param = new Dictionary<string, string> {
					{"mail_tel", mail}, {"password", pass}, {"auth_id", "15263781"}//dummy
				};
			} else {
				loginUrl = "https://secure.nicovideo.jp/secure/login?site=nicolive";
				param = new Dictionary<string, string> {
					{"mail", mail}, {"password", pass}
				};
			}
			
			try {
				var h = new Dictionary<string, string>();
				h.Add("Referer", "https://account.nicovideo.jp/login?site=niconico&next_url=%2F&sec=header_pc&cmnhd_ref=device%3Dpc%26site%3Dniconico%26pos%3Dheader_login%26page%3Dtop");
				h.Add("Content-Type", "application/x-www-form-urlencoded");
				h.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
				h.Add("User-Agent", util.userAgent);
				
				var _d = "mail_tel=" + WebUtility.UrlEncode(param["mail_tel"]) + "&password=" + WebUtility.UrlEncode(param["password"]) + "&auth_id=" + param["auth_id"];
				var d = Encoding.ASCII.GetBytes(_d);
				var cc = new CookieContainer();
				
				util.debugWriteLine(cc.GetCookieHeader(new Uri(loginUrl)));
				
				if (util.isUseCurl(CurlHttpVersion.CURL_HTTP_VERSION_1_1)) {
					var curlR = new Curl().getStr(loginUrl, h, CurlHttpVersion.CURL_HTTP_VERSION_2TLS, "POST", _d, true, true, false);
					if (curlR == null) {
						log += "ログインページに接続できませんでした:Curl";
						return null;
					}
					var m = new Regex("Set-Cookie: (.+?)=(.+?);").Matches(curlR);
					if (m.Count == 0) {
						log += "保存するクッキーがありませんでした:Curl";
						return null;
					}
					Cookie us = null, secureC = null;  
					foreach (Match _m in m) {
						if (_m.Groups[1].Value == "user_session") us = new Cookie(_m.Groups[1].Value, _m.Groups[2].Value);
						if (_m.Groups[1].Value == "user_session_secure") secureC = new Cookie(_m.Groups[1].Value, _m.Groups[2].Value);
					}
					if (us != null) {
						copyUserSession(cc, us, secureC);
						return cc;
					}
					
					var locationM = new Regex("Location: (.+)").Matches(curlR);
					if (locationM.Count == 0) {
						log += "ログイン接続の転送先が見つかりませんでした:Curl";
						return null;
					}
					var location = locationM[locationM.Count - 1].Groups[1].Value;
					location = util.getRegGroup(curlR, "Location: (.+)\r");
					if (location == null) {
						log += "not found location." + curlR;
						return null;
					}
					//location = WebUtility.UrlDecode(location);
					
					var setCookie = new Dictionary<string, string>();
					var setCookieM = new Regex("Set-Cookie: (.+?)=(.*?);").Matches(curlR);
					foreach (Match _m in setCookieM) {
						var key = _m.Groups[1].Value;
						if (setCookie.ContainsKey(key)) {
						    	if (_m.Groups[2].Value == "") setCookie.Remove(key);
						    	else if (_m.Groups[2].Value != "") setCookie[key] = _m.Groups[2].Value;
						    }
						else if (_m.Groups[2].Value != "") setCookie.Add(key, _m.Groups[2].Value);
					}
					h["Cookie"] = string.Join("; ", setCookie.Select(x => x.Key + "=" + x.Value).ToArray());
					h.Remove("Content-Type");
					var curlR2 = new Curl().getStr(location, h, CurlHttpVersion.CURL_HTTP_VERSION_1_1, "GET", null, true, true, false);
					
					var browName = util.getRegGroup(curlR2, "id=\"deviceNameInput\".+?value=\"(.+?)\"");
	                if (browName == null) browName = "Google Chrome (Windows)";
	                var mfaUrl = util.getRegGroup(curlR2, "<form action=\"(.+?)\"");
	                if (mfaUrl == null || mfaUrl.IndexOf("/mfa") == -1) {
	                	var notice = util.getRegGroup(curlR2, "\"notice__text\">(.+?)</p>");
						if (notice != null) log += " notice:" + notice;
	                	else log += "2段階認証のURLを取得できませんでした。:Curl";
						return null;
	                }
	                mfaUrl = "https://account.nicovideo.jp" + mfaUrl;
	                //mfaUrl = WebUtility.UrlDecode(mfaUrl);
	                var sendTo = util.getRegGroup(curlR2, "class=\"userAccount\">(.+?)</span>");
	                if (sendTo == null && util.getRegGroup(curlR2, "(スマートフォンのアプリを使って)") != null) {
	                	sendTo = "app";
	                }
	                var f = new MfaInputForm(sendTo);
	                
	                var dr = f.ShowDialog();
	                if (f.code == null) {
	                	log += "二段階認証のコードが入力されていませんでした:Curl";
	                	return null;
	                }
	                util.debugWriteLine(mfaUrl);
	                h["Referer"] = location;
	                h["Origin"] = "https://account.nicovideo.jp";
	                h["Content-Type"] = "application/x-www-form-urlencoded";
	                _d = "otp=" + f.code + "&loginBtn=%E3%83%AD%E3%82%B0%E3%82%A4%E3%83%B3&device_name=Google+Chrome+%28Windows%29";
	                var curlR3 = new Curl().getStr(mfaUrl, h, CurlHttpVersion.CURL_HTTP_VERSION_1_1, "POST", _d, true, true, false);
	                if (curlR3 == null) {
	                	log += "二段階認証のコードを正常に送信できませんでした:Curl";
	                	return null;
	                }
	                setCookieM = new Regex("Set-Cookie: (.+?)=(.*?);").Matches(curlR3);
					foreach (Match _m in setCookieM) {
						var key = _m.Groups[1].Value;
						if (setCookie.ContainsKey(key)) {
						    	if (_m.Groups[2].Value == "") setCookie.Remove(key);
						    	else setCookie[key] = _m.Groups[2].Value;
						    }
						else setCookie.Add(key, _m.Groups[2].Value);
					}
	                h["Cookie"] = string.Join("; ", setCookie.Select(x => x.Key + "=" + x.Value).ToArray());
	                var location2 = util.getRegGroup(curlR3, "Location: (.+)\r");
	                
	                var curlR4 = new Curl().getStr(location2, h, CurlHttpVersion.CURL_HTTP_VERSION_1_1, "GET", null, true, true, false);
	                m = new Regex("Set-Cookie: (.+?)=(.+?);").Matches(curlR4);
	                if (m.Count == 0) {
	                	log += "not set cookie." + curlR4 + ":Curl";
	                	return null;
	                }
					foreach (Match _m in m) {
						if (_m.Groups[1].Value == "user_session") us = new Cookie(_m.Groups[1].Value, _m.Groups[2].Value);
						if (_m.Groups[1].Value == "user_session_secure") secureC = new Cookie(_m.Groups[1].Value, _m.Groups[2].Value);
					}
					if (us != null) {
						//setUserSession(cc, us, secureC);
						copyUserSession(cc, us, secureC);
						return cc;
					}
					log += "not found session:Curl";
					return null; 
				}
				
				var r = util.sendRequest(loginUrl, h, d, "POST", cc);
				if (r == null) {
					log += "ログインページに接続できませんでした:default";
					return null;
				}
				var _cc = cc.GetCookies(new Uri(loginUrl));
				if (_cc["user_session"] != null) {
					//cc.Add(r.Cookies["user_session"]);
					return cc;
				}
				if (r.ResponseUri == null || !r.ResponseUri.AbsolutePath.StartsWith("/mfa")) {
					log += "ログインに失敗しました。:default";
					try {
						using (var sr = new StreamReader(r.GetResponseStream())) {
							var res = sr.ReadToEnd();
							var notice = util.getRegGroup(res, "\"notice__text\">(.+?)</p>");
							if (notice != null) log += " " + notice;
						}
					} catch (Exception e) {
						util.debugWriteLine(e.Message + e.Source + e.StackTrace);
					}
					return null;
				}
				using (var sr = new StreamReader(r.GetResponseStream())) {
					var res = sr.ReadToEnd();
					util.debugWriteLine(res);
					
					var browName = util.getRegGroup(res, "id=\"deviceNameInput\".+?value=\"(.+?)\"");
	                if (browName == null) browName = "Google Chrome (Windows)";
	                var mfaUrl = util.getRegGroup(res, "<form action=\"(.+?)\"");
	                if (mfaUrl == null) {
	                	log += "2段階認証のURLを取得できませんでした。:default";
						return null;
	                }
	                mfaUrl = "https://account.nicovideo.jp" + mfaUrl;
	                var sendTo = util.getRegGroup(res, "class=\"userAccount\">(.+?)</span>");
	                if (sendTo == null && util.getRegGroup(res, "(スマートフォンのアプリを使って)") != null) {
	                	sendTo = "app";
	                }
	                var f = new MfaInputForm(sendTo);
	                
	                var dr = f.ShowDialog();
	                if (f.code == null) {
	                	log += "二段階認証のコードが入力されていませんでした:default";
	                	return null;
	                }
	                util.debugWriteLine(mfaUrl);
	                h["Referer"] = r.ResponseUri.OriginalString;
	                h["Origin"] = "https://account.nicovideo.jp";
	                _d = "otp=" + f.code + "&loginBtn=%E3%83%AD%E3%82%B0%E3%82%A4%E3%83%B3&device_name=Google+Chrome+%28Windows%29";
	                d = Encoding.ASCII.GetBytes(_d);
	                var _r = util.sendRequest(mfaUrl, h, d, "POST", cc);
	                if (_r == null) {
	                	log += "二段階認証のコードを正常に送信できませんでした:default";
	                	return null;
	                }
	                using (var _sr = new StreamReader(_r.GetResponseStream())) {
	                	res = _sr.ReadToEnd();
	                	util.debugWriteLine(res);
	                }
	                _cc = cc.GetCookies(new Uri(loginUrl));
					if (_cc["user_session"] != null) {
						return cc;
	                } else {
	                	log += "2段階認証のログインに失敗しました:default";
	                	return null;
	                }
				}
			} catch (Exception e) {
				util.debugWriteLine(e.Message+e.StackTrace);
				log += e.Message + e.Source + e.StackTrace;
				return null;
			}
		}
		private CookieContainer copyUserSession(CookieContainer cc, 
				Cookie c, Cookie secureC) {
			if (c != null && c.Value != "") {
				cc.Add(TargetUrl, new Cookie(c.Name, c.Value));
				cc.Add(TargetUrl2, new Cookie(c.Name, c.Value));
				cc.Add(TargetUrl3, new Cookie(c.Name, c.Value));
				cc.Add(TargetUrl4, new Cookie(c.Name, c.Value));
			}
			if (secureC != null && secureC.Value != "") {
				cc.Add(TargetUrl, new Cookie(secureC.Name, secureC.Value));
				cc.Add(TargetUrl2, new Cookie(secureC.Name, secureC.Value));
				cc.Add(TargetUrl3, new Cookie(secureC.Name, secureC.Value));
				cc.Add(TargetUrl4, new Cookie(secureC.Name, secureC.Value));
			}
			return cc;
		}
	}
	
}
