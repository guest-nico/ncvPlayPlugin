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
				if (host.IsConnected) {
					si.Arguments = "lv" + host.GetPlayerStatus().LiveNum;
				}
			} catch (Exception e) {
				try {
					si.Arguments = host.GetLiveInfo().WssUrl;
				} catch (Exception ee) {
					
				}
                
            }
//			var path = Path.GetFullPath("視聴プラグイン/視聴プラグイン.exe");
			si.FileName = util.getJarPath()[0] + "/視聴プラグイン/視聴プラグイン.exe";
			si.RedirectStandardInput = true;
			si.UseShellExecute = false;
			_p.StartInfo = si;
			_p.Start();
			return _p;
		}
		void connectedEvent(object sender, EventArgs e) {
			try {
				var lv = "lv" + host.GetPlayerStatus().LiveNum;
				p.StandardInput.WriteLine(lv);
				p.StandardInput.Flush();
			} catch (Exception ee) {
				try {
                    var lv = host.GetLiveInfo().WssUrl;
                    p.StandardInput.WriteLine(lv);
                    p.StandardInput.Flush();
				} catch (Exception eee) {
					
				}
			}
		}
	}
}