using System.Runtime.InteropServices;
/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/04/06
 * Time: 20:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace namaichi
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolStripMenuItem NotifyQualityaudio_highMenu;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.qualityBox = new System.Windows.Forms.ComboBox();
			this.urlText = new System.Windows.Forms.TextBox();
			this.recBtn = new System.Windows.Forms.Button();
			this.groupLabel = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visualMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.formColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.characterColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.バージョン情報VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label12 = new System.Windows.Forms.Label();
			this.playerBtn = new System.Windows.Forms.Button();
			this.logText = new System.Windows.Forms.TextBox();
			this.latencyList = new System.Windows.Forms.ComboBox();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.notifyIconMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Notify30LatencyMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Notify15LatencyMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Notify10LatencyMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Notify05LatencyMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIconRecentSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.NotifyQualitysuper_lowMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyQualitylowMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyQualitynormalMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyQualityhighMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyQualitysuper_highMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.NotifyQualitySeparator = new System.Windows.Forms.ToolStripSeparator();
			this.closeNotifyIconMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.openNotifyIconMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			NotifyQualityaudio_highMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.notifyIconMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// NotifyQualityaudio_highMenu
			// 
			NotifyQualityaudio_highMenu.Name = "NotifyQualityaudio_highMenu";
			NotifyQualityaudio_highMenu.Size = new System.Drawing.Size(168, 22);
			NotifyQualityaudio_highMenu.Text = "画質:audio_high";
			NotifyQualityaudio_highMenu.Visible = false;
			NotifyQualityaudio_highMenu.Click += new System.EventHandler(this.notifyQualityMenuClick);
			// 
			// qualityBox
			// 
			this.qualityBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.qualityBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.qualityBox.FormattingEnabled = true;
			this.qualityBox.Location = new System.Drawing.Point(329, 38);
			this.qualityBox.Name = "qualityBox";
			this.qualityBox.Size = new System.Drawing.Size(79, 20);
			this.qualityBox.TabIndex = 19;
			this.qualityBox.TextChanged += new System.EventHandler(this.QualityBoxTextUpdate);
			// 
			// urlText
			// 
			this.urlText.Location = new System.Drawing.Point(69, 38);
			this.urlText.Margin = new System.Windows.Forms.Padding(2);
			this.urlText.Name = "urlText";
			this.urlText.Size = new System.Drawing.Size(180, 19);
			this.urlText.TabIndex = 0;
			// 
			// recBtn
			// 
			this.recBtn.Location = new System.Drawing.Point(279, 71);
			this.recBtn.Margin = new System.Windows.Forms.Padding(2);
			this.recBtn.Name = "recBtn";
			this.recBtn.Size = new System.Drawing.Size(75, 24);
			this.recBtn.TabIndex = 1;
			this.recBtn.TabStop = false;
			this.recBtn.Text = "録画開始";
			this.recBtn.UseVisualStyleBackColor = true;
			this.recBtn.Visible = false;
			this.recBtn.Click += new System.EventHandler(this.recBtnAction);
			// 
			// groupLabel
			// 
			this.groupLabel.Location = new System.Drawing.Point(0, 0);
			this.groupLabel.Name = "groupLabel";
			this.groupLabel.Size = new System.Drawing.Size(100, 23);
			this.groupLabel.TabIndex = 0;
			this.groupLabel.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.fileMenuItem,
									this.toolMenuItem,
									this.visualMenuItem,
									this.helpMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(470, 26);
			this.menuStrip1.TabIndex = 11;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileMenuItem
			// 
			this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.終了ToolStripMenuItem});
			this.fileMenuItem.Name = "fileMenuItem";
			this.fileMenuItem.ShowShortcutKeys = false;
			this.fileMenuItem.Size = new System.Drawing.Size(85, 22);
			this.fileMenuItem.Text = "ファイル(&F)";
			// 
			// 終了ToolStripMenuItem
			// 
			this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
			this.終了ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.終了ToolStripMenuItem.Text = "終了(&X)";
			this.終了ToolStripMenuItem.Click += new System.EventHandler(this.endMenu_Click);
			// 
			// toolMenuItem
			// 
			this.toolMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.optionMenuItem});
			this.toolMenuItem.Name = "toolMenuItem";
			this.toolMenuItem.ShowShortcutKeys = false;
			this.toolMenuItem.Size = new System.Drawing.Size(74, 22);
			this.toolMenuItem.Text = "ツール(&T)";
			// 
			// optionMenuItem
			// 
			this.optionMenuItem.Name = "optionMenuItem";
			this.optionMenuItem.ShowShortcutKeys = false;
			this.optionMenuItem.Size = new System.Drawing.Size(147, 22);
			this.optionMenuItem.Text = "オプション(&O)";
			this.optionMenuItem.Click += new System.EventHandler(this.optionItem_Select);
			// 
			// visualMenuItem
			// 
			this.visualMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.formColorMenuItem,
									this.characterColorMenuItem});
			this.visualMenuItem.Name = "visualMenuItem";
			this.visualMenuItem.Size = new System.Drawing.Size(62, 22);
			this.visualMenuItem.Text = "表示(&V)";
			// 
			// formColorMenuItem
			// 
			this.formColorMenuItem.Name = "formColorMenuItem";
			this.formColorMenuItem.Size = new System.Drawing.Size(182, 22);
			this.formColorMenuItem.Text = "ウィンドウの色(&W)";
			this.formColorMenuItem.Click += new System.EventHandler(this.FormColorMenuItemClick);
			// 
			// characterColorMenuItem
			// 
			this.characterColorMenuItem.Name = "characterColorMenuItem";
			this.characterColorMenuItem.Size = new System.Drawing.Size(182, 22);
			this.characterColorMenuItem.Text = "文字の色(&S)";
			this.characterColorMenuItem.Click += new System.EventHandler(this.CharacterColorMenuItemClick);
			// 
			// helpMenuItem
			// 
			this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.バージョン情報VToolStripMenuItem});
			this.helpMenuItem.Name = "helpMenuItem";
			this.helpMenuItem.ShowShortcutKeys = false;
			this.helpMenuItem.Size = new System.Drawing.Size(75, 22);
			this.helpMenuItem.Text = "ヘルプ(&H)";
			// 
			// バージョン情報VToolStripMenuItem
			// 
			this.バージョン情報VToolStripMenuItem.Name = "バージョン情報VToolStripMenuItem";
			this.バージョン情報VToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.バージョン情報VToolStripMenuItem.Text = "バージョン情報(&A)";
			this.バージョン情報VToolStripMenuItem.Click += new System.EventHandler(this.versionMenu_Click);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 41);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(58, 18);
			this.label12.TabIndex = 18;
			this.label12.Text = "放送URL";
			// 
			// playerBtn
			// 
			this.playerBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.playerBtn.Location = new System.Drawing.Point(259, 35);
			this.playerBtn.Name = "playerBtn";
			this.playerBtn.Size = new System.Drawing.Size(64, 24);
			this.playerBtn.TabIndex = 1;
			this.playerBtn.Text = "視聴";
			this.playerBtn.UseVisualStyleBackColor = true;
			this.playerBtn.Click += new System.EventHandler(this.PlayerBtnClick);
			// 
			// logText
			// 
			this.logText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.logText.Location = new System.Drawing.Point(6, 71);
			this.logText.Margin = new System.Windows.Forms.Padding(2);
			this.logText.Multiline = true;
			this.logText.Name = "logText";
			this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logText.Size = new System.Drawing.Size(453, 81);
			this.logText.TabIndex = 5;
			this.logText.TabStop = false;
			// 
			// latencyList
			// 
			this.latencyList.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.latencyList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.latencyList.FormattingEnabled = true;
			this.latencyList.Items.AddRange(new object[] {
									"0.5",
									"1.0",
									"1.5",
									"3.0"});
			this.latencyList.Location = new System.Drawing.Point(414, 38);
			this.latencyList.Name = "latencyList";
			this.latencyList.Size = new System.Drawing.Size(45, 20);
			this.latencyList.TabIndex = 19;
			this.latencyList.TextChanged += new System.EventHandler(this.latencyTextUpdate);
			// 
			// notifyIcon
			// 
			this.notifyIcon.BalloonTipTitle = "title";
			this.notifyIcon.ContextMenuStrip = this.notifyIconMenuStrip;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "ニコ生放送チェックツール（仮";
			this.notifyIcon.Visible = true;
			this.notifyIcon.DoubleClick += new System.EventHandler(this.NotifyIconDoubleClick);
			// 
			// notifyIconMenuStrip
			// 
			this.notifyIconMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.Notify30LatencyMenu,
									this.Notify15LatencyMenu,
									this.Notify10LatencyMenu,
									this.Notify05LatencyMenu,
									this.notifyIconRecentSeparator,
									NotifyQualityaudio_highMenu,
									this.NotifyQualitysuper_lowMenu,
									this.NotifyQualitylowMenu,
									this.NotifyQualitynormalMenu,
									this.NotifyQualityhighMenu,
									this.NotifyQualitysuper_highMenu,
									this.NotifyQualitySeparator,
									this.openNotifyIconMenu,
									this.toolStripSeparator1,
									this.closeNotifyIconMenu});
			this.notifyIconMenuStrip.Name = "notifyIconMenuStrip";
			this.notifyIconMenuStrip.Size = new System.Drawing.Size(169, 308);
			// 
			// Notify30LatencyMenu
			// 
			this.Notify30LatencyMenu.Name = "Notify30LatencyMenu";
			this.Notify30LatencyMenu.Size = new System.Drawing.Size(168, 22);
			this.Notify30LatencyMenu.Text = "遅延:3.0";
			this.Notify30LatencyMenu.Click += new System.EventHandler(this.notifyLatencyMenu);
			// 
			// Notify15LatencyMenu
			// 
			this.Notify15LatencyMenu.Name = "Notify15LatencyMenu";
			this.Notify15LatencyMenu.Size = new System.Drawing.Size(168, 22);
			this.Notify15LatencyMenu.Text = "遅延:1.5";
			this.Notify15LatencyMenu.Click += new System.EventHandler(this.notifyLatencyMenu);
			// 
			// Notify10LatencyMenu
			// 
			this.Notify10LatencyMenu.Name = "Notify10LatencyMenu";
			this.Notify10LatencyMenu.Size = new System.Drawing.Size(168, 22);
			this.Notify10LatencyMenu.Text = "遅延:1.0";
			this.Notify10LatencyMenu.Click += new System.EventHandler(this.notifyLatencyMenu);
			// 
			// Notify05LatencyMenu
			// 
			this.Notify05LatencyMenu.Name = "Notify05LatencyMenu";
			this.Notify05LatencyMenu.Size = new System.Drawing.Size(168, 22);
			this.Notify05LatencyMenu.Text = "遅延:0.5";
			this.Notify05LatencyMenu.Click += new System.EventHandler(this.notifyLatencyMenu);
			// 
			// notifyIconRecentSeparator
			// 
			this.notifyIconRecentSeparator.Name = "notifyIconRecentSeparator";
			this.notifyIconRecentSeparator.Size = new System.Drawing.Size(165, 6);
			// 
			// NotifyQualitysuper_lowMenu
			// 
			this.NotifyQualitysuper_lowMenu.Name = "NotifyQualitysuper_lowMenu";
			this.NotifyQualitysuper_lowMenu.Size = new System.Drawing.Size(168, 22);
			this.NotifyQualitysuper_lowMenu.Text = "画質:super_low";
			this.NotifyQualitysuper_lowMenu.Visible = false;
			this.NotifyQualitysuper_lowMenu.Click += new System.EventHandler(this.notifyQualityMenuClick);
			// 
			// NotifyQualitylowMenu
			// 
			this.NotifyQualitylowMenu.Name = "NotifyQualitylowMenu";
			this.NotifyQualitylowMenu.Size = new System.Drawing.Size(168, 22);
			this.NotifyQualitylowMenu.Text = "画質:low";
			this.NotifyQualitylowMenu.Visible = false;
			this.NotifyQualitylowMenu.Click += new System.EventHandler(this.notifyQualityMenuClick);
			// 
			// NotifyQualitynormalMenu
			// 
			this.NotifyQualitynormalMenu.Name = "NotifyQualitynormalMenu";
			this.NotifyQualitynormalMenu.Size = new System.Drawing.Size(168, 22);
			this.NotifyQualitynormalMenu.Text = "画質:normal";
			this.NotifyQualitynormalMenu.Visible = false;
			this.NotifyQualitynormalMenu.Click += new System.EventHandler(this.notifyQualityMenuClick);
			// 
			// NotifyQualityhighMenu
			// 
			this.NotifyQualityhighMenu.Name = "NotifyQualityhighMenu";
			this.NotifyQualityhighMenu.Size = new System.Drawing.Size(168, 22);
			this.NotifyQualityhighMenu.Text = "画質:high";
			this.NotifyQualityhighMenu.Visible = false;
			this.NotifyQualityhighMenu.Click += new System.EventHandler(this.notifyQualityMenuClick);
			// 
			// NotifyQualitysuper_highMenu
			// 
			this.NotifyQualitysuper_highMenu.Name = "NotifyQualitysuper_highMenu";
			this.NotifyQualitysuper_highMenu.Size = new System.Drawing.Size(168, 22);
			this.NotifyQualitysuper_highMenu.Text = "画質:super_high";
			this.NotifyQualitysuper_highMenu.Visible = false;
			this.NotifyQualitysuper_highMenu.Click += new System.EventHandler(this.notifyQualityMenuClick);
			// 
			// NotifyQualitySeparator
			// 
			this.NotifyQualitySeparator.Name = "NotifyQualitySeparator";
			this.NotifyQualitySeparator.Size = new System.Drawing.Size(165, 6);
			this.NotifyQualitySeparator.Visible = false;
			// 
			// closeNotifyIconMenu
			// 
			this.closeNotifyIconMenu.Name = "closeNotifyIconMenu";
			this.closeNotifyIconMenu.Size = new System.Drawing.Size(168, 22);
			this.closeNotifyIconMenu.Text = "終了";
			this.closeNotifyIconMenu.Click += new System.EventHandler(this.CloseNotifyIconMenuClick);
			// 
			// openNotifyIconMenu
			// 
			this.openNotifyIconMenu.Name = "openNotifyIconMenu";
			this.openNotifyIconMenu.Size = new System.Drawing.Size(168, 22);
			this.openNotifyIconMenu.Text = "開く";
			this.openNotifyIconMenu.Click += new System.EventHandler(this.OpenNotifyIconMenuClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(470, 163);
			this.Controls.Add(this.latencyList);
			this.Controls.Add(this.qualityBox);
			this.Controls.Add(this.playerBtn);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.logText);
			this.Controls.Add(this.recBtn);
			this.Controls.Add(this.urlText);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "ニコ生新配信録画ツール（仮 ver0.86.15";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_Close);
			this.Load += new System.EventHandler(this.mainForm_Load);
			this.SizeChanged += new System.EventHandler(this.MainFormSizeChanged);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.notifyIconMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem openNotifyIconMenu;
		private System.Windows.Forms.ToolStripMenuItem NotifyQualitysuper_highMenu;
		private System.Windows.Forms.ToolStripMenuItem NotifyQualityhighMenu;
		private System.Windows.Forms.ToolStripMenuItem NotifyQualitynormalMenu;
		private System.Windows.Forms.ToolStripMenuItem NotifyQualitylowMenu;
		private System.Windows.Forms.ToolStripMenuItem NotifyQualitysuper_lowMenu;
		private System.Windows.Forms.ToolStripMenuItem Notify05LatencyMenu;
		private System.Windows.Forms.ToolStripMenuItem Notify10LatencyMenu;
		private System.Windows.Forms.ToolStripMenuItem Notify15LatencyMenu;
		private System.Windows.Forms.ToolStripMenuItem Notify30LatencyMenu;
		private System.Windows.Forms.ToolStripMenuItem closeNotifyIconMenu;
		private System.Windows.Forms.ToolStripSeparator NotifyQualitySeparator;
		private System.Windows.Forms.ToolStripSeparator notifyIconRecentSeparator;
		private System.Windows.Forms.ContextMenuStrip notifyIconMenuStrip;
		public System.Windows.Forms.NotifyIcon notifyIcon;
		public System.Windows.Forms.ComboBox latencyList;
		private System.Windows.Forms.ToolStripMenuItem characterColorMenuItem;
		private System.Windows.Forms.ToolStripMenuItem formColorMenuItem;
		private System.Windows.Forms.ToolStripMenuItem visualMenuItem;
		public System.Windows.Forms.ComboBox qualityBox;
		public System.Windows.Forms.Button playerBtn;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ToolStripMenuItem バージョン情報VToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem optionMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		public System.Windows.Forms.TextBox logText;
		public System.Windows.Forms.Button recBtn;
		public System.Windows.Forms.TextBox urlText;
		private System.Windows.Forms.Label groupLabel;
	}
}