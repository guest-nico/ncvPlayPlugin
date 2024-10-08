﻿/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/04/06
 * Time: 20:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using SunokoLibrary.Application;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Threading;
using namaichi.rec;
using namaichi.config;
using namaichi.play;
using namaichi.utility;
using SuperSocket.ClientEngine;

//using System.Diagnostics.Process;

namespace namaichi
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		
		public rec.RecordingManager rec;
		//private bool isInitRun = true;
		private namaichi.config.config config = new namaichi.config.config();
		private string[] args;
		private play.Player player;
		//private string labelUrl;
		private Thread madeThread;
		
		public MainForm(string[] args)
		{
			madeThread = Thread.CurrentThread;
			
			#if !DEBUG
				FormBorderStyle = FormBorderStyle.FixedSingle;
//				if (config.get("IsLogFile") == "true") 
//					config.set("IsLogFile", "false");
			#endif
					
			System.Diagnostics.Debug.Listeners.Clear();
			System.Diagnostics.Debug.Listeners.Add(new Logger.TraceListener());
		    
			InitializeComponent();
			Text = "視聴プラグイン（仮 " + util.versionStr;
			
			this.args = args;
			
			rec = new rec.RecordingManager(this, config);
			player = new Player(this, config);
			
			if (Array.IndexOf(args, "-stdIO") > -1) util.isStdIO = true;
			
			var lv = (args.Length == 0) ? null : util.getRegGroup(args[0], "(lv\\d+(,\\d+)*)");
			util.setLog(config, lv);
			
			

			util.debugWriteLine("arg len " + args.Length);
			util.debugWriteLine("arg join " + string.Join(" ", args));
			
			
            //nicoSessionComboBox1.Selector.PropertyChanged += Selector_PropertyChanged;
//            checkBoxShowAll.Checked = bool.Parse(config.get("isAllBrowserMode"));
			//if (isInitRun) initRec();
			try {
				//Width = int.Parse(config.get("Width"));
				//Height = int.Parse(config.get("Height"));
				Width = 492;
				Height = 201;
			} catch (Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
			}
			try {
				var x = config.get("X");
				var y = config.get("Y");
				if (x != "" && y != "") {
					StartPosition = FormStartPosition.Manual;
					Location = new Point(int.Parse(x), int.Parse(y));
				}
			} catch (Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
				StartPosition = FormStartPosition.WindowsDefaultLocation;
			}
			
			setBackColor(Color.FromArgb(int.Parse(config.get("recBackColor"))));
			setForeColor(Color.FromArgb(int.Parse(config.get("recForeColor"))));
			
			if (bool.Parse(config.get("Isminimized"))) {
				WindowState = FormWindowState.Minimized;
				if (bool.Parse(config.get("IsMinimizeNotify"))) {
					Visible = false;
					ShowInTaskbar = false;
				}
			}
			
			var qr = config.get("qualityRank").Split(',').ToList();
			var qrD = config.defaultConfig["qualityRank"].Split(',');
			if (qr.Count != qrD.Length) {
				foreach (var q in qrD) 
					if (qr.IndexOf(q) == -1) qr.Add(q);
			}
			config.set("qualityRank", string.Join(",", qr.ToArray()));
		}

		private void recBtnAction(object sender, EventArgs e) {
			rec.isClickedRecBtn = true;
			rec.rec();
			
		}
		/*
		async void Selector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
			
			
	
			
            switch(e.PropertyName)
            {
                case "SelectedIndex":
                    var cookieContainer = new CookieContainer();
                    var currentGetter = nicoSessionComboBox1.Selector.SelectedImporter;
                    if (currentGetter != null)
                    {
//                        var result = await currentGetter.GetCookiesAsync(TargetUrl);
                        
//                        var cookie = result.Status == CookieImportState.Success ? result.Cookies["user_session"] : null;
                        
                        //logText.Text += cookie.Name + cookie.Value+ cookie.Expires;
                        
                        //UI更新
//                        txtCookiePath.Text = currentGetter.SourceInfo.CookiePath;
//                        btnOpenCookieFileDialog.Enabled = true;
//                        txtUserSession.Text = cookie != null ? cookie.Value : null;
//                        txtUserSession.Enabled = result.Status == CookieImportState.Success;
                        //Properties.Settings.Default.SelectedSourceInfo = currentGetter.SourceInfo;
                        //Properties.Settings.Default.Save();
//                        config.set("browserNum", nicoSessionComboBox1.Selector.SelectedIndex.ToString());
//                        if (cookie != null) config.set("user_session", cookie.Value);
//                        config.set("isAllBrowserMode", nicoSessionComboBox1.Selector.IsAllBrowserMode.ToString());
                    }
                    else
                    {
//                        txtCookiePath.Text = null;
//                        txtUserSession.Text = null;
//                        txtUserSession.Enabled = false;
//                        btnOpenCookieFileDialog.Enabled = false;
                    }
                    break;
            }
        }

		void btnReload_Click(object sender, EventArgs e)
        { 
			util.debugWriteLine(DateTime.Now.ToString("{W}"));
			var si = nicoSessionComboBox1.Selector.SelectedImporter.SourceInfo;
			util.debugWriteLine(si.EngineId + " " + si.BrowserName + " " + si.ProfileName);
//			var a = new SunokoLibrary.Application.Browsers.FirefoxImporterFactory();
//			foreach (var b in a.GetCookieImporters()) {
//				var c = b.GetCookiesAsync(TargetUrl);
//				c.ConfigureAwait(false);
				
//				util.debugWriteLine(c.Result.Cookies["user_session"]);
//			}
			util.debugWriteLine(nicoSessionComboBox1.Selector.SelectedImporter.SourceInfo.CookiePath);
			//System.IO.Directory.CreateDirectory("aa/ss/u");
			//a.GetCookieImporter(new CookieSourceInfo("
			//var tsk = nicoSessionComboBox1.Selector.UpdateAsync(); 
		}
        void btnOpenCookieFileDialog_Click(object sender, EventArgs e)
        { var tsk = nicoSessionComboBox1.ShowCookieDialogAsync(); }
        void checkBoxShowAll_CheckedChanged(object sender, EventArgs e)
        { nicoSessionComboBox1.Selector.IsAllBrowserMode = checkBoxShowAll.Checked;
        	//config.set("isAllBrowserMode", nicoSessionComboBox1.Selector.IsAllBrowserMode.ToString());
        }
        void playBtn_Click(object sender, EventArgs e)
        { player.play();}
        */
        void optionItem_Select(object sender, EventArgs e)
        { 
        	try {
	        	optionForm o = new optionForm(config); o.ShowDialog();
	        } catch (Exception ee) {
        		util.debugWriteLine(ee.Message + " " + ee.StackTrace);
	        }
        }
        
        /*
        public async Task<Cookie> getCookie() {
        	var cookieContainer = new CookieContainer();
            var currentGetter = nicoSessionComboBox1.Selector.SelectedImporter;
            if (currentGetter != null)
            {
            	
            	var result = await currentGetter.GetCookiesAsync(TargetUrl).ConfigureAwait(false);
                var cookie = result.Status == CookieImportState.Success ? result.Cookies["user_session"] : null;
                //logText.Text += cookie.Name + cookie.Value+ cookie.Expires;
                return cookie;
            }
            else return null;
        }
        */
        public void addLogText(string t, bool isInvoke = true) {
       		if (util.isStdIO) Console.WriteLine("info.log:" + t);
       		if (!util.isShowWindow) return;
       		try {
	       		if (this.IsDisposed) return;
	       		if (!this.IsHandleCreated) return;
	       		if (isInvoke) {
		        	this.Invoke((MethodInvoker)delegate() {
		       		       	try {
				        	    string _t = "";
						    	if (logText.Text.Length != 0) _t += "\r\n";
						    	_t += t;
						    	
					    		logText.AppendText(_t);
								if (logText.Text.Length > 200000) 
									logText.Text = logText.Text.Substring(logText.TextLength - 10000);
		       		       	} catch (Exception e) {
		       		       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
		       		       	}
		
					});
	       		} else {
	       			try {
		        	    string _t = "";
				    	if (logText.Text.Length != 0) _t += "\r\n";
				    	_t += t;
				    	
			    		logText.AppendText(_t);
						if (logText.Text.Length > 20000) 
							logText.Text = logText.Text.Substring(logText.TextLength - 10000);
	   		       	} catch (Exception e) {
	   		       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	   		       	}
	       		}
	       	} catch (Exception e) {
	       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	}
       		
		}
        public void addLogTextTest(string t) {
       		addLogText(t);
        }
		void endMenu_Click(object sender, EventArgs e)
		{
			try {
				Close();
			} catch (Exception ee) {
				util.debugWriteLine(ee.Message + " " + ee.StackTrace + " " + ee.TargetSite);
			}
				
//			if (kakuninClose()) Close();;
		}
		
		void form_Close(object sender, FormClosingEventArgs e)
		{
			if (!kakuninClose()) e.Cancel = true;
		}
		bool kakuninClose() {
			if (rec.rfu != null) {
				var _m = (rec.isPlayOnlyMode) ? "視聴" : "録画";
				DialogResult res = MessageBox.Show(_m + "中ですが終了しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (res == DialogResult.No) return false;
			}
			try{
				util.debugWriteLine("width " + Width.ToString() + " height " + Height.ToString() + " restore width " + RestoreBounds.Width.ToString() + " restore height " + RestoreBounds.Height.ToString());
				if (this.WindowState == FormWindowState.Normal) {
					config.set("Width", Width.ToString());
					config.set("Height", Height.ToString());
					config.set("X", Location.X.ToString());
					config.set("Y", Location.Y.ToString());
				} else {
					config.set("Width", RestoreBounds.Width.ToString());
					config.set("Height", RestoreBounds.Height.ToString());
					config.set("X", RestoreBounds.X.ToString());
					config.set("Y", RestoreBounds.Y.ToString());
				}

			} catch(Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace);
			}
			player.stopPlaying(true, true);
			return true;
		}
		
		
		
		void PlayerBtnClick(object sender, EventArgs e)
		{
			rec.isClickedRecBtn = true;
			player.play();
		}
		public void setPlayerBtnEnable(bool b) {
			if (!util.isShowWindow) return;
			try {
				if (!IsDisposed) {
		        	Invoke((MethodInvoker)delegate() {
						try {
					        playerBtn.Enabled = b;
						} catch (Exception e) {
		       	       		util.debugWriteLine("player btn enabled exception " + e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	       		}
					});
				}
			} catch (Exception e) {
	       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	}
		}
		
		
		void mainForm_Load(object sender, EventArgs e)
		{
			if (!util.isShowWindow) return;
			
			var a = util.getJarPath();
			var desc = System.Diagnostics.FileVersionInfo.GetVersionInfo(util.getJarPath()[0] + "/websocket4net.dll");
			if (desc.FileDescription != "WebSocket4Net for .NET 4.5 gettable data bytes") {
				Invoke((MethodInvoker)delegate() {
					System.Windows.Forms.MessageBox.Show("「WebSocket4Net.dll」をver0.86.9以降に同梱されているものと置き換えてください");
				});
			}
			
			
			
			//.net
			var ver = util.Get45PlusFromRegistry();  
			if (ver < 4.52) {
				
				Task.Run(() => {
				    Invoke((MethodInvoker)delegate() {
						var b = new DotNetMessageBox(ver);
						b.Show(this); 
//						System.Windows.Forms.MessageBox.Show("「動作には.NET 4.5.2以上が推奨です。現在は" + ver + "です。");
					});
				});
			}
			
			var osName = config.get("osName");
			if (osName == "") {
				osName = util.CheckOSName();
				config.set("osName", osName);
			}
			util.osName = osName;
			
			var osType = config.get("osType");
			if (osType == "") {
				osType = util.CheckOSType();
				config.set("osType", osType);
			}
			util.debugWriteLine("OS: " + util.osName);
			util.debugWriteLine("OSType: " + osType);
				
			if (args.Length > 0) {
				var ar = new ArgReader(args, config, this);
				ar.read();
				if (ar.isConcatMode) {
					urlText.Text = string.Join("|", args);
	            	rec.rec();
				} else {
					
					config.argConfig = ar.argConfig;
					rec.argTsConfig = ar.tsConfig;
					rec.isRecording = true;
//					rec.setArgConfig(args);
					if (ar.lvid != null || ar.wssUrl != null) {
						urlText.Text = ar.lvid != null ? ar.lvid : ar.wssUrl;
						player.play();
					}
					
					//if (ar.isPlayMode) player.play();
					//else rec.rec();
				}
				if (bool.Parse(config.get("Isminimized"))) {
					this.WindowState = FormWindowState.Minimized;
				}
            }
			setLatencyListText(config.get("latency"));
			
			startStdRead();
		}
		
		void versionMenu_Click(object sender, EventArgs e)
		{
			var v = new VersionForm(this);
			v.ShowDialog();
		}
		void startStdRead() {
			Task.Run(() => {
	         	while (true) {
			        Thread.Sleep(1000);
					var a = Console.ReadLine();
					if (a == null || a.Length == 0) continue;
					util.debugWriteLine("std in " + a);
					var lvid = util.getRegGroup(a, "(lv\\d+)");
					var _wssUrl = util.getRegGroup(a, "(wss://[^,\\s]+)");
					if ((lvid != null || _wssUrl != null) && urlText.Enabled) {
						urlText.Text = lvid != null ? lvid : _wssUrl;
						player.play();
					}
				}
			});
		}
		
		
		
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			try {
				var url = e.Data.GetData(DataFormats.Text).ToString();
				if (urlText.Enabled) urlText.Text = url;
			} catch (Exception ee) {
				util.debugWriteLine(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
		}
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("UniformResourceLocator") ||
			    e.Data.GetDataPresent("UniformResourceLocatorW") ||
			    e.Data.GetDataPresent(DataFormats.Text)) {
				util.debugWriteLine(e.Effect);
				e.Effect = DragDropEffects.Copy;
				
			}
		}
		public void setQualityList(string[] l, string recQuality) {
			
			try {
				if (!IsDisposed) {
					int qualityCount; string qualityText;
					getQualityListInfo(out qualityCount, out qualityText);
					var isChange = false;
					var isTextChange = false;
					if (l.Length != qualityCount) isChange = true;
					else {
						for(var i = 0; i < l.Length; i++)
							if (l[i] != qualityBox.Items[i].ToString()) isChange = true;
					}
					if (qualityText != recQuality) isTextChange = true;
					if (!isChange && !isTextChange) return;
							
		        	Invoke((MethodInvoker)delegate() {
						try {
					       	if (isChange) {
					        	qualityBox.Items.Clear();
								foreach (var _l in l) qualityBox.Items.Add(_l);
					       	}
					       	qualityBox.Tag = "set";
							qualityBox.Text = recQuality;
							
							//notify menu
							if (isChange) {
								foreach (var _m in notifyIconMenuStrip.Items) {
									if (!(_m is ToolStripMenuItem)) continue;
									var m = (ToolStripMenuItem)_m;
									
									if (m.Name.IndexOf("Quality") > -1) {
										var visible = false;
										foreach  (var _l in l) {
											if (m.Name.IndexOf(_l) > -1) visible = true;
										}
										m.Visible = visible;
										
										var original = util.getRegGroup(m.Text, "(.+[a-z])");
										m.Text = original + (m.Text.IndexOf(recQuality) > -1 ? 
												"(現在)" : "");
									}
								}
								NotifyQualitySeparator.Visible = l.Length != 0;
							}
							
						} catch (Exception e) {
		       	       		util.debugWriteLine("player btn enabled exception " + e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	       		}
					});
				}
			} catch (Exception e) {
	       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	}
			
			
		}
		public bool getQualityListInfo(out int count, out string text) {
			var ret = false;
			count = -1; text = null;
			var _count = -1; string _text = null;
			try {
				if (!IsDisposed) {
		        	Invoke((MethodInvoker)delegate() {
						try {
					       	_count = qualityBox.Items.Count;
					       	_text = qualityBox.Text;
					       	ret = true;
						} catch (Exception e) {
		       	       		util.debugWriteLine("player btn enabled exception " + e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	       		}
					});
					count = _count; text = _text;
				}
			} catch (Exception e) {
	       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	}
			return ret;
		}
		
		void QualityBoxTextUpdate(object sender, EventArgs e)
		{
			util.debugWriteLine("quality box text Update " + qualityBox.SelectedIndex + " a " + qualityBox.Text + " b " + qualityBox.Tag);
			if (qualityBox.Text == "") return;
			if (qualityBox.Tag != null && qualityBox.Tag.ToString() == "set") {
				qualityBox.Tag = null;
				return;
			}
			if (rec.wsr != null)
				rec.wsr.setQuality(qualityBox.Text);
			
			setQualityNotifyText(qualityBox.Text);
		}
		public void setPlayerBtnText(string s) {
			if (!util.isShowWindow) return;
			try {
				if (!IsDisposed) {
		        	Invoke((MethodInvoker)delegate() {
						try {
					        playerBtn.Text = s;
						} catch (Exception e) {
		       	       		util.debugWriteLine("player btn enabled exception " + e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	       		}
					});
				}
			} catch (Exception e) {
	       		util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       	}
		}
				void FormColorMenuItemClick(object sender, EventArgs e)
		{
			var d = new ColorDialog();
			var r = d.ShowDialog();
			if (r == DialogResult.OK) {
				setBackColor(d.Color);
				config.set("recBackColor", d.Color.ToArgb().ToString());
			}
		}
		private void setBackColor(Color color) {
			BackColor = //commentList.BackgroundColor = 
				color;
			//if (color != 
		}
		void CharacterColorMenuItemClick(object sender, EventArgs e)
		{
			var d = new ColorDialog();
			var r = d.ShowDialog();
			if (r == DialogResult.OK) {
				setForeColor(d.Color);
				config.set("recForeColor", d.Color.ToArgb().ToString());
			}
		}
		private void setForeColor(Color color) {
			var c = getChildControls(this);
			foreach (var _c in c)
				if (_c.GetType() == typeof(GroupBox) ||
				    _c.GetType() == typeof(Label)) _c.ForeColor = color;
		}
		private List<Control> getChildControls(Control c) {
			util.debugWriteLine("cname " + c.Name);
			var ret = new List<Control>();
			foreach (Control _c in c.Controls) {
				var children = getChildControls(_c);
				ret.Add(_c);
				ret.AddRange(children);
				util.debugWriteLine(c.Name + " " + children.Count);
			}
			util.debugWriteLine(c.Name + " " + ret.Count);
			return ret;
		}
		void latencyTextUpdate(object sender, EventArgs e)
		{
			util.debugWriteLine("latency text Update " + latencyList.SelectedIndex + " a " + latencyList.Text);
			if (latencyList.Text == "") return;
			if (rec.wsr != null)
				rec.wsr.setLatency(latencyList.Text);
			setLatencyListText(latencyList.Text);
		}
		public bool formAction(Action a, bool isAsync = true) {
			if (IsDisposed || !util.isShowWindow) return false;
			
			if (Thread.CurrentThread == madeThread) {
				try {
					a.Invoke();
				} catch (Exception e) {
					util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
					return false;
				}
			} else {
				try {
					var r = BeginInvoke((MethodInvoker)delegate() {
						try {    
				       		a.Invoke();
				       	} catch (Exception e) {
							util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
						}
					});
					if (!isAsync) 
						EndInvoke(r);
				} catch (Exception e) {
					util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
					return false;
				} 
			}
			return true;
		}
		public string getLatencyText() {
			string ret = null; 
			formAction(() => {
				ret = latencyList.Text;
			}, false);
			return ret;
		}
		void CloseNotifyIconMenuClick(object sender, EventArgs e)
		{
			Close();
		}
		
		void notifyLatencyMenu(object sender, EventArgs e)
		{
			try {
				var name = util.getRegGroup(((ToolStripMenuItem)sender).Name, "(\\d+)");
				name = name.Insert(1, ".");
				latencyList.Text = name;
			} catch (Exception ee) {
				util.debugWriteLine(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
		}
		
		void notifyQualityMenuClick(object sender, EventArgs e)
		{
			try {
				var name = util.getRegGroup(((ToolStripMenuItem)sender).Name, "Quality(.+?)Menu");
				qualityBox.Text = name;
			} catch (Exception ee) {
				util.debugWriteLine(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
		}
		void setLatencyListText(string s) {
			latencyList.Text = s;
			foreach (var item in notifyIconMenuStrip.Items) {
				if (!(item is ToolStripMenuItem)) continue;
				var i = (ToolStripMenuItem)item;
				if (i.Text.IndexOf("遅延") == -1) continue;
				var original = util.getRegGroup(i.Text, "(.+\\d)");
				i.Text = original + (i.Text.IndexOf(s) > -1 ? 
						"(現在)" : "");
			}
		}
		void setQualityNotifyText(string s) {
			foreach (var item in notifyIconMenuStrip.Items) {
				if (!(item is ToolStripMenuItem)) continue;
				var i = (ToolStripMenuItem)item;
				if (i.Text.IndexOf("画質") == -1) continue;
				var original = util.getRegGroup(i.Text, "(.+[a-z])");
				i.Text = original + (i.Text.IndexOf(s) > -1 ? 
						"(現在)" : "");
			}
		}
		
		void MainFormSizeChanged(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized &&
			    	bool.Parse(config.get("IsMinimizeNotify")))
				Visible = false;
		}
		void NotifyIconDoubleClick(object sender, EventArgs e)
		{
			activateForm();
		}
		void activateForm() {
			Visible = true;
			ShowInTaskbar = true;
			if (WindowState == FormWindowState.Minimized) {
				WindowState = FormWindowState.Normal;
			}
			Activate();
			
		}
		void OpenNotifyIconMenuClick(object sender, EventArgs e)
		{
			activateForm();
		}
		public void setSamune(string url) {
       		if (!util.isShowWindow) return;
       		if (IsDisposed) return;
       		WebClient cl = new WebClient();
       		cl.Proxy = null;
			
       		System.Drawing.Icon icon =  null;
			try {
       			byte[] pic;
       			if (url.IndexOf("nicochannel.jp") == -1)
       				pic = cl.DownloadData(url);
       			else {
       				string d = null;
       				var h = new Dictionary<string, string>() {
						{"User-Agent", util.userAgent}
					};
       				pic = new Curl().getBytes(url, h, CurlHttpVersion.CURL_HTTP_VERSION_2TLS, "GET", d, false);
       				if (pic == null) return;
       			}
				
       			using (var st = new System.IO.MemoryStream(pic)) {
					icon = Icon.FromHandle(new System.Drawing.Bitmap(st).GetHicon());
       			}
				
			} catch (Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
				return;
			}
			
       		formAction(() => {
				try {
					if (bool.Parse(config.get("IstitlebarSamune"))) {
	        	    	this.Icon = icon;
					}
				} catch (Exception e) {
	       			util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
	       		}
			});
		}
		public void setTitle(string s) {
			formAction(() => {
				try {
					Text = s;
				} catch (Exception e) {
					util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
		       	}
			});
		}
	}
}
