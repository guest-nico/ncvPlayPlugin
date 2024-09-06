/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/09/22
 * Time: 4:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace namaichi
{
	/// <summary>
	/// Description of VersionForm.
	/// </summary>
	public partial class VersionForm : Form
	{
		MainForm form;
		public VersionForm(MainForm form)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			this.form = form;
			versionLabel.Text = util.versionStr + " (" + util.versionDayStr + ")";
			//communityLinkLabel.Links.Add(0, communityLinkLabel.Text.Length, "http://com.nicovideo.jp/community/co2414037");
		}
		void okBtnClick(object sender, EventArgs e)
		{
			Close();
		}
		void communityLinkLabel_Click(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://com.nicovideo.jp/community/co2414037");
		}
		void DownloadPageLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://guest-nico.github.io/pages/downloads.html");
		}
		void VersionFormLoad(object sender, EventArgs e)
		{
			Task.Factory.StartNew(() => checkLastVersion());
		}
		private void checkLastVersion() {
			var r = util.getPageSource("https://github.com/guest-nico/ncvPlayPlugin/commits/master/");
			if (r == null) {
				form.formAction(() =>
						lastVersionLabel.Text = "最新の利用可能なバージョンを確認できませんでした");
				return;
			}
			var m = new Regex("https://github.com/guest-nico/ncvPlayPlugin/releases/download/releases/(.+?).(zip|rar)").Match(r);
			if (!m.Success) {
				form.formAction(() =>
						lastVersionLabel.Text = "最新の利用可能なバージョンが見つかりませんでした");
				return;
			}
			var v = m.Groups[1].Value;
			if (v.IndexOf(util.versionStr) > -1)
				form.formAction(() => lastVersionLabel.Text = "ニコ生視聴ツール（仮は最新バージョンです");
			else {
				form.formAction(() => {
                	lastVersionLabel.Text = v + "が利用可能です";
                	lastVersionLabel.Links.Clear();
                	lastVersionLabel.Links.Add(0, v.Length, m.Value);
                });
			}
		}
		void LastVersionLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			util.debugWriteLine("click");
			if (e.Button == MouseButtons.Left) {
				if (e.Link != null && e.Link.Length != 0) {
					System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
				}
			}
		}
	}
}
