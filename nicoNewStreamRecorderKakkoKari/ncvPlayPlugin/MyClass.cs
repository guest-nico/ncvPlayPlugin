/*
 * Created by SharpDevelop.
 * User: user
 * Date: 2018/12/25
 * Time: 0:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace ncvPlayPlugin
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class MyClass : Plugin.IPlugin
	{

        Plugin.IPluginHost host = null;
        Process p = null;
		Plugin.IPluginHost Plugin.IPlugin.Host {
			get {
        		return host;
			}
			set {
        		host = value;
			}
		}
		bool Plugin.IPlugin.IsAutoRun {
			get {
        		return false;
				throw new NotImplementedException();
			}
		}
		
		string Plugin.IPlugin.Description {
			get {
        		return "HLSおよびRTMPで視聴します";
				throw new NotImplementedException();
			}
		}
		
		string Plugin.IPlugin.Version {
			get {
        		return "1.0";
				throw new NotImplementedException();
			}
		}
		
		string Plugin.IPlugin.Name {
			get {
        		//var path = Path.GetFullPath("視聴プラグイン/視聴プラグイン.exe");
//        		var path = util.getJarPath()[0] + "/視聴プラグイン/視聴プラグイン.exe";
//				return path;
        		return "視聴プラグイン";
				throw new NotImplementedException();
			}
		}
		
		void Plugin.IPlugin.AutoRun()
		{
			return;
			throw new NotImplementedException();
		}
		
		void Plugin.IPlugin.Run()
		{
			if (p == null) {
				p = getProcess();
				
				host.BroadcastConnected += new Plugin.BroadcastConnectedEventHandler(connectedEvent);
				Task.Run(() => {
					p.WaitForExit();
					p = null;
				});
				
			}
		}
		Process getProcess() {
            var _p = new Process();
			var si = new ProcessStartInfo();
			
			try {
				if (host != null && host.IsConnected) 
					si.Arguments = host.GetLiveInfo().WssUrl;
				//			var path = Path.GetFullPath("視聴プラグイン/視聴プラグイン.exe");
				si.FileName = util.getJarPath()[0] + "/視聴プラグイン/視聴プラグイン.exe";
				si.RedirectStandardInput = true;
				si.UseShellExecute = false;
				_p.StartInfo = si;
			} catch (Exception ee) {
				Debug.WriteLine(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
			try {
				if (host != null && host.IsConnected) {
					var _cc = new List<System.Windows.Forms.Control>();
					getControls(host.MainForm, _cc);
					foreach(var _c in _cc) {
						if (_c == null) continue;
						var lv = util.getRegGroup(_c.Text, "(lv\\d+)");
						
						if (lv != null && _c.Parent != null && _c.Parent.Name.IndexOf("tool") > -1) {
							_p.StartInfo.Arguments = lv;
						}
					}
				}
			} catch (Exception ee) {
				Debug.WriteLine(ee.Message + ee.StackTrace + ee.Source);
				MessageBox.Show("e " + ee.Message + " " + ee.StackTrace);
			}
			_p.Start();
			return _p;
		}
		void getControls(System.Windows.Forms.Control c, List<System.Windows.Forms.Control> ret) {
			foreach (System.Windows.Forms.Control cc in c.Controls) {
				if (cc.Controls.Count != 0) getControls(cc, ret);
				ret.Add(cc);
			}
		}
		void connectedEvent(object sender, EventArgs e) {
			try {
				var lv = "";
				if (host != null && host.IsConnected) {
					var _cc = new List<System.Windows.Forms.Control>();
					getControls(host.MainForm, _cc);
					foreach(var _c in _cc) {
						if (_c == null) continue;
						var _lv = util.getRegGroup(_c.Text, "(lv\\d+)");
						
						if (lv != null && _c.Parent != null && _c.Parent.Name.IndexOf("tool") > -1)
							lv += _lv + " "; 
					}
				}
				//var lv = host.GetHeartBeat().LiveNum;//.GetLiveInfo().WssUrl;
                p.StandardInput.WriteLine(lv);
                p.StandardInput.Flush();
			} catch (Exception eee) {
				Debug.WriteLine(eee.Message + eee.Source + eee.StackTrace + eee.TargetSite);
			}
		}
	}
}