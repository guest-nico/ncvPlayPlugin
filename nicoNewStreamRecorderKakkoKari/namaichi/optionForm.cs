/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/05/06
 * Time: 20:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

using namaichi.config;
using SunokoLibrary.Application;
using SunokoLibrary.Windows.ViewModels;

namespace namaichi
{
	/// <summary>
	/// Description of optionForm.
	/// </summary>
	public partial class optionForm : Form
	{
		private config.config cfg;
		
		static readonly Uri TargetUrl = new Uri("http://live.nicovideo.jp/");
		private string fileNameFormat;
		
		public optionForm(config.config cfg)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.StartPosition = FormStartPosition.CenterParent;
			//util.debugWriteLine(p.X + " " + p.Y);
			InitializeComponent();
			//this.Location = p;
			this.cfg = cfg;
			
			nicoSessionComboBox1.Selector.PropertyChanged += Selector_PropertyChanged;
//			nicoSessionComboBox2.Selector.PropertyChanged += Selector2_PropertyChanged;
			setFormFromConfig();
			
			setBackColor(Color.FromArgb(int.Parse(cfg.get("recBackColor"))));
			setForeColor(Color.FromArgb(int.Parse(cfg.get("recForeColor"))));
		}
		
		void hozonFolderSanshouBtn_Click(object sender, EventArgs e)
		{
			var f = new FolderBrowserDialog();
			DialogResult r = f.ShowDialog();
			util.debugWriteLine(f.SelectedPath);
//			recordDirectoryText.Text = f.SelectedPath;
		}
		
		void fileNameOptionBtn(object sender, EventArgs e)
		{
			
		}
		void FileNameDokujiSetteiBtn_Click(object sender, EventArgs e)
		{
			var a = new fileNameOptionForm(fileNameFormat);
			var res = a.ShowDialog();
			if (res != DialogResult.OK) return;
//			fileNameTypeDokujiSetteiBtn.Text = util.getFileNameTypeSample(a.ret);
			fileNameFormat = a.ret;
		}
		
		void optionOk_Click(object sender, EventArgs e)
		{
			var formData = getFormData();
			cfg.saveFromForm(formData);
			Close();
			
			//main cookie
			var importer = nicoSessionComboBox1.Selector.SelectedImporter;
			if (importer == null || importer.SourceInfo == null) return;
			var si = importer.SourceInfo;
			
			if (isCookieFileSiteiChkBox.Checked)
				SourceInfoSerialize.save(si.GenerateCopy(si.BrowserName, si.ProfileName, cookieFileText.Text), false);
			else SourceInfoSerialize.save(si, false); 
			
			

		}

		private Dictionary<string, string> getFormData() {
			//var selectedImporter = nicoSessionComboBox1.Selector.SelectedImporter;
//			var browserName = (selectedImporter != null) ? selectedImporter.SourceInfo.BrowserName : "";
			var browserNum = (useCookieRadioBtn.Checked) ? "2" : "1";
//			var browserNum2 = (useCookieRadioBtn2.Checked) ? "2" : "1";
			return new Dictionary<string, string>(){
				{"accountId",mailText.Text},
				{"accountPass",passText.Text},
				//{"user_session",passText.Text},
				{"browserNum",browserNum},
//				{"isAllBrowserMode",checkBoxShowAll.Checked.ToString().ToLower()},
				{"issecondlogin",useSecondLoginChkBox.Checked.ToString().ToLower()},
				{"qualityRank",getQualityRank()},
				{"EngineMode",getEngineMode()},
				{"latency",latencyList.Text},
				{"anotherPlayerPath",anotherPlayerPathText.Text},
				
				{"cookieFile",cookieFileText.Text},
				{"iscookie",isCookieFileSiteiChkBox.Checked.ToString().ToLower()},
				{"user_session",""},
				{"user_session_secure",""},
				
				{"Isminimized",isMinimizedChkBox.Checked.ToString().ToLower()},
				{"IsMinimizeNotify",isMinimizeNotifyChkBox.Checked.ToString().ToLower()},
			};
			
		}
		
		async void Selector_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
			//if (isInitRun) initRec();
			
            switch(e.PropertyName)
            {
                case "SelectedIndex":
                    var cookieContainer = new CookieContainer();
                    var currentGetter = nicoSessionComboBox1.Selector.SelectedImporter;
                    if (currentGetter != null)
                    {
                        var result = await currentGetter.GetCookiesAsync(TargetUrl);
                        
                        var cookie = result.Status == CookieImportState.Success ? result.Cookies["user_session"] : null;
//                        foreach (var c in result.Cookies)
//                        	util.debugWriteLine(c);
                        //logText.Text += cookie.Name + cookie.Value+ cookie.Expires;
                        
                        //UI更新
//                        txtCookiePath.Text = currentGetter.SourceInfo.CookiePath;
//                        btnOpenCookieFileDialog.Enabled = true;
//                        txtUserSession.Text = cookie != null ? cookie.Value : null;
//                        txtUserSession.Enabled = result.Status == CookieImportState.Success;
                        //Properties.Settings.Default.SelectedSourceInfo = currentGetter.SourceInfo;
                        //Properties.Settings.Default.Save();
                        //cfg.set("browserNum", nicoSessionComboBox1.Selector.SelectedIndex.ToString());
                        //if (cookie != null) cfg.set("user_session", cookie.Value);
                        //cfg.set("isAllBrowserMode", nicoSessionComboBox1.Selector.IsAllBrowserMode.ToString());
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
			//var si = nicoSessionComboBox1.Selector.SelectedImporter.SourceInfo;
			//util.debugWriteLine(si.EngineId + " " + si.BrowserName + " " + si.ProfileName);
//			var a = new SunokoLibrary.Application.Browsers.FirefoxImporterFactory();
//			foreach (var b in a.GetCookieImporters()) {
//				var c = b.GetCookiesAsync(TargetUrl);
//				c.ConfigureAwait(false);
				
//				util.debugWriteLine(c.Result.Cookies["user_session"]);
//			}
				
//			a.GetCookieImporter(new CookieSourceInfo("
			var tsk = nicoSessionComboBox1.Selector.UpdateAsync(); 
		}
		
        void btnOpenCookieFileDialog_Click(object sender, EventArgs e)
        { var tsk = nicoSessionComboBox1.ShowCookieDialogAsync(); }
        void checkBoxShowAll_CheckedChanged(object sender, EventArgs e)
        { nicoSessionComboBox1.Selector.IsAllBrowserMode = checkBoxShowAll.Checked;
        }

        private void setFormFromConfig() {
        	mailText.Text = cfg.get("accountId");
        	passText.Text = cfg.get("accountPass");
        	
        	if (cfg.get("browserNum") == "1") useAccountLoginRadioBtn.Checked = true;
        	else useCookieRadioBtn.Checked = true; 

        	setInitQualityRankList(cfg.get("qualityRank"));

        	setEngineType(cfg.get("EngineMode"));
//        	isDefaultEngineChkBox_UpdateAction();

//			setPlayerType();

			anotherPlayerPathText.Text = cfg.get("anotherPlayerPath");
//			isUsePlayerChkBox.Checked = bool.Parse(cfg.get("IsUsePlayer"));
//			isUsePlayerChkBox_UpdateAction();
			latencyList.Text = cfg.get("latency");
			
        	isCookieFileSiteiChkBox.Checked = bool.Parse(cfg.get("iscookie"));
        	isCookieFileSiteiChkBox_UpdateAction();
        	cookieFileText.Text = cfg.get("cookieFile");
        		
        	var si = SourceInfoSerialize.load(false);
        	nicoSessionComboBox1.Selector.SetInfoAsync(si);
        	
        	isMinimizedChkBox.Checked = bool.Parse(cfg.get("Isminimized"));
        	isMinimizeNotifyChkBox.Checked = bool.Parse(cfg.get("IsMinimizeNotify"));
        }
        
		void optionCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		
		void cookieFileSiteiSanshouBtn_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			var result = dialog.ShowDialog();
			if (result != DialogResult.OK) return;
			
			cookieFileText.Text = dialog.FileName;
		}
		
		void isCookieFileSiteiChkBox_CheckedChanged(object sender, EventArgs e)
		{
			isCookieFileSiteiChkBox_UpdateAction();
		}
		
		void isCookieFileSiteiChkBox_UpdateAction() {
//			cookieFileText.Enabled = isCookieFileSiteiChkBox.Checked;
//			cookieFileSanshouBtn.Enabled = isCookieFileSiteiChkBox.Checked;
		}
		
		async void loginBtn_Click(object sender, EventArgs e)
		{
			
			var cg = new rec.CookieGetter(cfg);
			var cc = await cg.getAccountCookie(mailText.Text, passText.Text);
			if (cc == null) {
				MessageBox.Show("login error", "", MessageBoxButtons.OK);
				return;
			}
			if (cc.GetCookies(TargetUrl)["user_session"] == null &&
				                   cc.GetCookies(TargetUrl)["user_session_secure"] == null)
				MessageBox.Show("no login", "", MessageBoxButtons.OK);
			else MessageBox.Show("login ok", "", MessageBoxButtons.OK);
			
			//MessageBox.Show("aa");
		}
		
		void highRankBtn_Click(object sender, EventArgs e)
		{
			int[] ranks = {6,0,1,2,3,4,5};
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks, qualityListBox));
		}
		void lowRankBtn_Click(object sender, EventArgs e)
		{
			int[] ranks = {5, 4, 3, 2, 1, 0, 6};
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks, qualityListBox));
		}
		public object[] getRanksToItems(int[] ranks, ListBox owner) {
			var items = new Dictionary<int, string> {
				{0, "3Mbps(super_high)"},
				{1, "2Mbps(high)"}, {2, "1Mbps(normal)"},
				{3, "384kbps(low)"}, {4, "192kbps(super_low)"},
				{5, "音声のみ(audio_high)"}, {6, "6Mbps(6Mbps1080p30fps)"}
			};
//			var ret = new ListBox.ObjectCollection(owner);
			var ret = new List<object>();
			for (int i = 0; i < ranks.Length; i++) {
				ret.Add((i + 1) + ". " + items[ranks[i]]);
			}
			return ret.ToArray();
		}
		void UpBtn_Click(object sender, EventArgs e)
		{
			var selectedIndex = qualityListBox.SelectedIndex;
			if (selectedIndex < 1) return;
			
			var ranks = getItemsToRanks(qualityListBox.Items);
			var selectedVal = ranks[selectedIndex + 0];
			ranks.RemoveAt(selectedIndex);
			var addIndex = (selectedIndex == 0) ? 0 : (selectedIndex - 1);
			ranks.Insert(addIndex, selectedVal);
			
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks.ToArray(), qualityListBox));
			qualityListBox.SetSelected(addIndex, true);
		}
		void DownBtn_Click(object sender, EventArgs e)
		{
			var selectedIndex = qualityListBox.SelectedIndex;
			if (selectedIndex > 4) return;
			
			var ranks = getItemsToRanks(qualityListBox.Items);
			var selectedVal = ranks[selectedIndex + 0];
			ranks.RemoveAt(selectedIndex);
			var addIndex = (selectedIndex == 5) ? 5 : (selectedIndex + 1);
			ranks.Insert(addIndex, selectedVal);
			
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks.ToArray(), qualityListBox));
			qualityListBox.SetSelected(addIndex, true);
		}
		List<int> getItemsToRanks(ListBox.ObjectCollection items) {
			var itemsDic = new Dictionary<int, string> {
				{0, "3Mbps(super_high)"},
				{1, "2Mbps(high)"}, {2, "1Mbps(normal)"},
				{3, "384kbps(low)"}, {4, "192kbps(super_low)"},
				{5, "音声のみ(audio_high)"}, {6, "6Mbps(6Mbps1080p30fps)"} 
			};
			var ret = new List<int>();
			for (int i = 0; i < items.Count; i++) {
				foreach (KeyValuePair <int, string> p in itemsDic)
					if (p.Value.IndexOf(((string)items[i]).Substring(3)) > -1) ret.Add(p.Key);
			}
			return ret;
		}
		string getQualityRank() {
			var buf = getItemsToRanks(qualityListBox.Items);
			var ret = "";
			foreach (var r in buf) {
				if (ret != "") ret += ",";
				ret += r;
			}
			return ret;
		}
		void setInitQualityRankList(string qualityRank) {
			var ranks = new List<int>();
			foreach (var r in qualityRank.Split(','))
				ranks.Add(int.Parse(r));
//			ranks.AddRange(qualityRank.Split(','));
			
			qualityListBox.Items.Clear();
			var items = getRanksToItems(ranks.ToArray(), qualityListBox);
			qualityListBox.Items.AddRange(items);
		}
		
		void setEngineType(string EngineMode) {
//			if (EngineMode == "0") isDefaultEngineChkBox.Checked = true;
			if (EngineMode == "1") isAnotherEngineChkBox.Checked = true;
			else if (EngineMode == "2") isRtmpEngine.Checked = true;
			else isAnotherEngineChkBox.Checked = true;
//			else isNoEngine.Checked = true;
//			isDefaultEngineChkBox_UpdateAction();
		}

		void anotherPlayerSanshouBtn_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			var result = dialog.ShowDialog();
			if (result != DialogResult.OK) return;
			
			anotherPlayerPathText.Text = dialog.FileName;
		}

		string getEngineMode() {
			if (isAnotherEngineChkBox.Checked) return "1";
			if (isRtmpEngine.Checked) return "2";
//			if (isNoEngine.Checked) return "3";
//			if (isDefaultEngineChkBox.Checked) return "0";
			return "1";
		}
		private void setBackColor(Color color) {
			BackColor = color;
			var c = getChildControls(this);
			foreach (var _c in c)
				if (//_c.GetType() == typeof(GroupBox) ||
				    _c.GetType() == typeof(System.Windows.Forms.Panel) || 
				    _c.GetType() == typeof(System.Windows.Forms.Form) 
				   	//_c.GetType() == typeof(System.Windows.Forms.TabPage) ||
				   //	_c.GetType() == typeof(System.Windows.Forms.TabControl)
				   )
						_c.BackColor = color;
		}
		private void setForeColor(Color color) {
			var c = getChildControls(this);
			foreach (var _c in c)
				if (//_c.GetType() == typeof(GroupBox) ||
				    _c.GetType() == typeof(Label) ||
				    _c.GetType() == typeof(CheckBox) ||
				   	_c.GetType() == typeof(RadioButton)) _c.ForeColor = color;
			
		}
		private List<Control> getChildControls(Control c) {
			util.debugWriteLine("cname " + c.Name);
			var ret = new List<Control>();
			foreach (Control _c in c.Controls) {
				ret.Add(_c);
				if (_c.GetType() != typeof(GroupBox)) {
					var children = getChildControls(_c);
					ret.AddRange(children);
				   }
				//util.debugWriteLine(c.Name + " " + children.Count);
			}
			util.debugWriteLine(c.Name + " " + ret.Count);
			return ret;
		}
	}
}
