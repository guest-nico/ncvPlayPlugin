﻿/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/05/03
 * Time: 20:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.IO.Pipes;

using namaichi.rec;
using namaichi;



namespace namaichi.play
{
	/// <summary>
	/// Description of Player.
	/// </summary>
	public class Player
	{
		private MainForm form;
		private config.config config;
		private Process process = null;
		private Process process2 = null;
		private Process commentProcess = null;
		string lastPlayUrl = null;
		private defaultFFplayController ctrl = null;
		//private commentForm commentForm = null;
		private bool isDefaultPlayer = false;
		private bool isDefaultCommentPlayer = false;
		
		private bool isRecording = false;
		public bool isReconnect = false;
		
		private bool isUsePlayer = false;
		private bool isUseCommentViewer = false;
		
		public StreamWriter pipeWriter;
		
		public Player(MainForm form, config.config config)
		{
			this.form = form;
			this.config = config;
			
		}
		public void play() {
			util.debugWriteLine("play");
			isUsePlayer = true;
			isUseCommentViewer = false;
			
			if (form.playerBtn.Text == "視聴") {
				setPlayerBtnText("視聴停止");
				lastPlayUrl = null;
				
				Task.Run(() => {
		         	if (!getHlsUrl()) {
		         		//end();
		         		form.Invoke((MethodInvoker)delegate() {
		         			setPlayerBtnText("視聴");
		         			form.recBtn.Enabled = true;
						});
		         		//form.rec.isPlayOnlyMode = false;
		         		return;
		         	}
					
					if (isUsePlayer)
						Task.Run(() => videoPlay(true));
					
				});
			} else {
				end();
				
			}
		}
		private void end() {
			Task.Run(() => {
			    setPlayerBtnText("視聴");
				stopPlaying(true, true);
				if (isDefaultPlayer && isUsePlayer) ctrlFormClose();
				//if (isDefaultCommentPlayer && isUseCommentViewer) defaultCommentFormClose();
				
				form.Invoke((MethodInvoker)delegate() {
					if (!form.recBtn.Enabled) {
						form.recBtn.Enabled = true;
						form.rec.rec();
					}
				    //form.rec.isPlayOnlyMode = false;
				});
			});
		}
		private void videoPlay(bool isStart) {
			isRecording = true;
			isDefaultPlayer = false;
			var rfu = form.rec.rfu;
			if (isStart) {
				//Task.Run(() => {
					var isStarted = false;
					while (form.playerBtn.Text == "視聴停止") {
		         		if (form.rec.hlsUrl == "end") {
		         			form.rec.hlsUrl = null;
		         			Thread.Sleep(15000);
		         			stopPlaying(true, false);
		         			if (isDefaultPlayer) ctrlFormClose();
		         			break;
				        }
						if (form.rec.hlsUrl == "timeshift") {
							form.addLogText("RTMPのタイムシフトはツールでの視聴ができません。");
		         			form.rec.hlsUrl = null;
		         			stopPlaying(true, true);
		         			if (isDefaultPlayer) ctrlFormClose();
		         			break;
				        }
						
						if (!isPlayable() && !isStarted) {
							Thread.Sleep(500);
							continue;
						} else isStarted = true;
						
						lastPlayUrl = form.rec.hlsUrl;
						
						//isRecording = true;
						sendPlayCommand(isDefaultPlayer);
						while (process == null) Thread.Sleep(500);
						
						
							while (true) {
								Thread.Sleep(1000);
								try {
									if ((form.rec.hlsUrl == "end" ||
									     form.rec.hlsUrl == null) && 
								        process.HasExited) 
											break;
									if (form.playerBtn.Text != "視聴停止") break;
									if (form.rec.rfu == null) 
										break;
								    
									
									//var isChangeUrlOk = !DefaultPlayer || form.rec.hlsUrl != lastPlayUrl;
									if (form.rec.hlsUrl == null) continue;
									var isRtmp = form.rec.hlsUrl.StartsWith("-vr");
									var isRtmpEnd = false;
//									var _isGetPlayerStatusAccessOk = isGetPlayerStatusAccessOk(out isRtmpEnd);
									if (isRtmp && isRtmpEnd) break;
									
									//var isOk = (isRtmp) ? _isGetPlayerStatusAccessOk :
									var isOk = (isRtmp) ? true :
										form.rec.hlsUrl != lastPlayUrl;
									if (isOk && form.rec.hlsUrl != null    
										&& (form.rec.hlsUrl.StartsWith("http") || form.rec.hlsUrl.StartsWith("-vr") || form.rec.hlsUrl.StartsWith("rtmp://")) || isReconnect) {
										isReconnect = false;
										
										stopPlaying(true, false);
		
										lastPlayUrl = form.rec.hlsUrl;
										sendPlayCommand(isDefaultPlayer);
		//								if (isDefaultPlayer) {
		//									ctrlFormClose();
		//								}
										var aaa = process.HasExited;
									}
								} catch (Exception e) {
									util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
								}
							}
						
						stopPlaying(true, false);
				    	if (isDefaultPlayer) ctrlFormClose();
				    	//isRecording = false;
				    	
						break;
					}
				    isRecording = false;
				    if (rfu == form.rec.rfu)
				    	setPlayerBtnText("視聴");
				    
				//});
				
			} else {
				
			}
			
		}
		private void sendPlayCommand(bool isDefaultPlayer) {
			Environment.SetEnvironmentVariable("SDL_AUDIODRIVER", "directsound", EnvironmentVariableTarget.Process);
			if (form.rec.hlsUrl.IndexOf("reconnecting") > -1 || form.rec.hlsUrl.IndexOf("end") > -1 || form.rec.hlsUrl == null) return;
			if (form.rec.hlsUrl.StartsWith("http"))
				playCommand(config.get("anotherPlayerPath"), form.rec.hlsUrl);
			else
				playCommandStd(config.get("anotherPlayerPath"), form.rec.hlsUrl);
			
		}
		
		
		bool isPlayable() {
			//return form.rec.hlsUrl != null &&
			//		form.rec.hlsUrl != lastPlayUrl;
			return form.rec.hlsUrl != null &&
					form.rec.hlsUrl.IndexOf("reconnecting") == -1 &&
					form.rec.hlsUrl.IndexOf("end") == -1;
		}
		private void playCommand(string exe, string args) {
			process = new System.Diagnostics.Process();
			process.StartInfo.FileName = exe;
//			process.StartInfo.RedirectStandardOutput = true;
//			process.StartInfo.RedirectStandardError = true;
			if (isDefaultPlayer) {
				process.StartInfo.RedirectStandardInput = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
			}

			util.debugWriteLine("playcommand args " + args);
			process.StartInfo.Arguments = args;
			
			try {
				process.Start();
				Thread.Sleep(1000);
//				setPipeName(process);
				
			} catch (Exception ee) {
				util.debugWriteLine(ee.Message + ee.StackTrace);
				form.addLogText("プレイヤーを開始できませんでした " + exe + " " + args);
			}
		}
		private void playCommandStd(string exe, string args) {
			try {
				
				var arg = (args.StartsWith("http")) ? ("-i " + form.rec.hlsUrl + " -f matroska -") : form.rec.hlsUrl;
				process = new Process();
				var si = new ProcessStartInfo();
				si.FileName = (args.StartsWith("http")) ? "ffmpeg.exe" : "rtmpdump.exe";
				si.Arguments = arg;
				si.RedirectStandardOutput = true;
				si.UseShellExecute = false;
				si.CreateNoWindow = true;
				process.StartInfo = si;
				process.Start();
				
				
				process2 = new Process();
				var ffmpegSi = new ProcessStartInfo();
//				ffmpegSi.FileName = "vlc.exe";
//				exe = "C:\\Users\\zack\\Downloads\\MPC-HomeCinema.1.4.2824.0.x86\\MPC-HomeCinema.1.4.2824.0.x86\\mpc-hc.exe";
				ffmpegSi.FileName = exe;
//				ffmpegSi.FileName = "C:\\Users\\zack\\Desktop\\c#project\\nicoNewStreamRecorderKakkoKariRepo2 10.12 tuujou s\\nicoNewStreamRecorderKakkoKari\\namaichi\\bin\\Debug\\mpc\\mpc-be.exe";
//				ffmpegSi.FileName = "C:\\Users\\zack\\Downloads\\MPC-HC.1.7.13.x64\\MPC-HC.1.7.13.x64\\mpc-hc64.exe";
//				var ffmpegArg = "- /new";
				
				var ffmpegArg = "-";
//				ffmpegArg = "-i " + args;
				if (exe.ToLower().IndexOf("mpc-hc") > -1) ffmpegArg += " /new";
				ffmpegSi.Arguments = ffmpegArg;
				ffmpegSi.RedirectStandardInput = true;
				ffmpegSi.RedirectStandardOutput = true;
				ffmpegSi.UseShellExecute = false;
				ffmpegSi.CreateNoWindow = true;
				process2.StartInfo = ffmpegSi;
				process2.Start();
				Thread.Sleep(1000);
				if (isDefaultPlayer)
					setPipeName(process2);
				
				var o = process.StandardOutput.BaseStream;
				var _is = process2.StandardInput.BaseStream;
				
	//			var f = new FileStream("aa.ts", FileMode.Create);
				var head = new byte[16*16];
				var isFirst = true;
				var cc = 0;
				
				var d = DateTime.Now;
//				Task.Run(() => readFfmpeg(ffmpegP));
				var b = new byte[100000000];
				while (!process.HasExited && !process2.HasExited) {
					try {
						var i = o.Read(b, 0, b.Length);
	//					if (isFirst) 
//						Debug.WriteLine(i);
						/*
						using (var _wf = new FileStream(cc + ".flv", FileMode.Create)) {
							_wf.Write(b, 0, i);
							_wf.Flush();
							cc++;
						}
						*/
						
						_is.Write(b, 0, i);
						_is.Flush();
							
					} catch (Exception ee) {
						Debug.WriteLine(ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
					}
				}
			} catch (Exception eee) {
				Debug.WriteLine(eee.Message + eee.Source + eee.StackTrace + eee.TargetSite);
			}
			stopPlaying(true, false);
			
		}
		private void commentCommand(string exe, string args) {
			commentProcess = new System.Diagnostics.Process();
			commentProcess.StartInfo.FileName = exe;
//			process.StartInfo.RedirectStandardOutput = true;
//			process.StartInfo.RedirectStandardError = true;
//			process.StartInfo.RedirectStandardInput = true;
//			process.StartInfo.UseShellExecute = false;
//			process.StartInfo.CreateNoWindow = true;
			util.debugWriteLine(args);
			commentProcess.StartInfo.Arguments = args;
			
			try {
				commentProcess.Start();
				
			} catch (Exception ee) {
				util.debugWriteLine("comment exception " + ee.Message + ee.StackTrace);
				form.addLogText("コメントビューワーを開始できませんでした " + exe + " " + args);
			}
		}
		public void stopPlaying(bool isVideoStop, bool isCommentStop) {
			if (isVideoStop) {
				stopProcessCore(process);
				stopProcessCore(process2);
			}
			if (isCommentStop)
				stopProcessCore(commentProcess);
			
				
		}
		private void stopProcessCore(Process p) {
			try {
				if (p != null && !p.HasExited) 
					p.Kill();
				
			} catch (Exception ee) {
				util.debugWriteLine("stop process " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
		}
		private void setPlayerBtnText(string s) {
			if (s == "視聴") 
				util.debugWriteLine(s);
			form.setPlayerBtnText(s);

		}
		private void ctrlFormClose() {
			if (ctrl == null || ctrl.IsDisposed) return;
			try {
				ctrl.Invoke((MethodInvoker)delegate() {
					try {
				    	ctrl.Close();
				    } catch (Exception e) {
				    	util.debugWriteLine("ctrl close exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
	           		}
				});
			} catch (Exception ee) {
				util.debugWriteLine("ctrl close2 exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
			ctrl = null;
		}
		
		
		private bool getHlsUrl() {
			if (form.rec.rfu == null) {
				form.rec.hlsUrl = null;
				form.rec.isPlayOnlyMode = true;
				form.rec.rec();
				form.Invoke((MethodInvoker)delegate() {
					form.recBtn.Enabled = false;
				});
				if (form.rec.rfu == null) return false;
				while(form.rec.rfu != null) {
					if (form.rec.hlsUrl == "end") return false;
					if (form.rec.hlsUrl != null) {
						
						return true;
					}
					Thread.Sleep(300);
				}
				return false;
			}
			return true;
		}
		private void setPipeName(Process p) {
			var pn = ((int)(new Random().NextDouble() * 10000)).ToString();
			p.StandardInput.WriteLine(pn);
			p.StandardInput.Flush();
			Thread.Sleep(1000);
			var server = new NamedPipeClientStream(pn);
			server.Connect();
		    pipeWriter = new StreamWriter(server);
			
	//                while (server.IsConnected) {
//        	pipeWriter.WriteLine();
//        	pipeWriter.Flush();
			
		}
		private bool isGetPlayerStatusAccessOk(out bool isRtmpEnd) {
			isRtmpEnd = false;
			try {
				string res = null;
				while (true) {
					util.debugWriteLine("isgetplayerstatus Access " + form.rec.rfu);
					util.debugWriteLine("isgetplayerstatus Access " + form.rec.rfu.container);
					util.debugWriteLine("isgetplayerstatus Access " + form.urlText.Text);
					res = util.getPageSource(form.urlText.Text,  form.rec.rfu.container, null, false, 5);
					if (res == null) {
						Thread.Sleep(2000);
						continue;
					}
					break;
				}
				var isTimeShift = false;
				var pageType = util.getPageTypeRtmp(res, ref isTimeShift, false);
				if (res != null && pageType != 0) isRtmpEnd = true;;
				if (res != null && res.IndexOf("http") > -1) return true; 
			} catch (Exception e) {
				
			}
			return false;
		}
	}
}
