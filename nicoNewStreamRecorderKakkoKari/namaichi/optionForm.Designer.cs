/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/05/06
 * Time: 20:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace namaichi
{
	partial class optionForm
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.anotherPlayerPathText = new System.Windows.Forms.TextBox();
			this.anotherPlayerSanshouBtn = new System.Windows.Forms.Button();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.downBtn = new System.Windows.Forms.Button();
			this.upBtn = new System.Windows.Forms.Button();
			this.qualityListBox = new System.Windows.Forms.ListBox();
			this.lowRankBtn = new System.Windows.Forms.Button();
			this.highRankBtn = new System.Windows.Forms.Button();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.isRtmpEngine = new System.Windows.Forms.RadioButton();
			this.isAnotherEngineChkBox = new System.Windows.Forms.RadioButton();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cookieFileText = new System.Windows.Forms.TextBox();
			this.useAccountLoginRadioBtn = new System.Windows.Forms.RadioButton();
			this.useCookieRadioBtn = new System.Windows.Forms.RadioButton();
			this.passText = new System.Windows.Forms.TextBox();
			this.mailText = new System.Windows.Forms.TextBox();
			this.nicoSessionComboBox1 = new namaichi.NicoSessionComboBox2();
			this.isCookieFileSiteiChkBox = new System.Windows.Forms.CheckBox();
			this.checkBoxShowAll = new System.Windows.Forms.CheckBox();
			this.loginBtn = new System.Windows.Forms.Button();
			this.cookieFileSanshouBtn = new System.Windows.Forms.Button();
			this.btnReload = new System.Windows.Forms.Button();
			this.useSecondLoginChkBox = new System.Windows.Forms.CheckBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.latencyList = new System.Windows.Forms.ComboBox();
			this.label14 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(9, 10);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(411, 485);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.Click += new System.EventHandler(this.btnReload_Click);
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.groupBox1);
			this.tabPage6.Controls.Add(this.groupBox8);
			this.tabPage6.Controls.Add(this.groupBox7);
			this.tabPage6.Controls.Add(this.groupBox10);
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new System.Drawing.Size(403, 459);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "視聴設定";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.anotherPlayerPathText);
			this.groupBox8.Controls.Add(this.anotherPlayerSanshouBtn);
			this.groupBox8.Location = new System.Drawing.Point(5, 341);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(385, 53);
			this.groupBox8.TabIndex = 20;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "プレイヤー";
			// 
			// anotherPlayerPathText
			// 
			this.anotherPlayerPathText.Location = new System.Drawing.Point(18, 22);
			this.anotherPlayerPathText.Name = "anotherPlayerPathText";
			this.anotherPlayerPathText.Size = new System.Drawing.Size(297, 19);
			this.anotherPlayerPathText.TabIndex = 19;
			// 
			// anotherPlayerSanshouBtn
			// 
			this.anotherPlayerSanshouBtn.Location = new System.Drawing.Point(321, 20);
			this.anotherPlayerSanshouBtn.Margin = new System.Windows.Forms.Padding(2);
			this.anotherPlayerSanshouBtn.Name = "anotherPlayerSanshouBtn";
			this.anotherPlayerSanshouBtn.Size = new System.Drawing.Size(40, 23);
			this.anotherPlayerSanshouBtn.TabIndex = 17;
			this.anotherPlayerSanshouBtn.Text = "参照";
			this.anotherPlayerSanshouBtn.UseVisualStyleBackColor = true;
			this.anotherPlayerSanshouBtn.Click += new System.EventHandler(this.anotherPlayerSanshouBtn_Click);
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.downBtn);
			this.groupBox7.Controls.Add(this.upBtn);
			this.groupBox7.Controls.Add(this.qualityListBox);
			this.groupBox7.Controls.Add(this.lowRankBtn);
			this.groupBox7.Controls.Add(this.highRankBtn);
			this.groupBox7.Location = new System.Drawing.Point(5, 82);
			this.groupBox7.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox7.Size = new System.Drawing.Size(385, 197);
			this.groupBox7.TabIndex = 3;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "HLS画質優先順位";
			// 
			// downBtn
			// 
			this.downBtn.Location = new System.Drawing.Point(290, 63);
			this.downBtn.Name = "downBtn";
			this.downBtn.Size = new System.Drawing.Size(42, 30);
			this.downBtn.TabIndex = 4;
			this.downBtn.Text = "下へ";
			this.downBtn.UseVisualStyleBackColor = true;
			this.downBtn.Click += new System.EventHandler(this.DownBtn_Click);
			// 
			// upBtn
			// 
			this.upBtn.Location = new System.Drawing.Point(290, 27);
			this.upBtn.Name = "upBtn";
			this.upBtn.Size = new System.Drawing.Size(42, 30);
			this.upBtn.TabIndex = 4;
			this.upBtn.Text = "上へ";
			this.upBtn.UseVisualStyleBackColor = true;
			this.upBtn.Click += new System.EventHandler(this.UpBtn_Click);
			// 
			// qualityListBox
			// 
			this.qualityListBox.FormattingEnabled = true;
			this.qualityListBox.ItemHeight = 12;
			this.qualityListBox.Items.AddRange(new object[] {
									"1. 自動(abr) (実験放送:auto)",
									"2. 3Mbps(super_high) (実験放送:ultrahigh)",
									"3. 2Mbps(high・高画質) (実験放送:superhigh)",
									"4. 1Mbps(normal・低画質) (実験放送:high)",
									"5. 384kbps(low) (実験放送:middle)",
									"6. 192kbps(super_low) (実験放送:low)"});
			this.qualityListBox.Location = new System.Drawing.Point(26, 27);
			this.qualityListBox.Name = "qualityListBox";
			this.qualityListBox.Size = new System.Drawing.Size(247, 112);
			this.qualityListBox.TabIndex = 3;
			// 
			// lowRankBtn
			// 
			this.lowRankBtn.Location = new System.Drawing.Point(129, 156);
			this.lowRankBtn.Name = "lowRankBtn";
			this.lowRankBtn.Size = new System.Drawing.Size(86, 23);
			this.lowRankBtn.TabIndex = 1;
			this.lowRankBtn.Text = "なるべく低画質";
			this.lowRankBtn.UseVisualStyleBackColor = true;
			this.lowRankBtn.Click += new System.EventHandler(this.lowRankBtn_Click);
			// 
			// highRankBtn
			// 
			this.highRankBtn.Location = new System.Drawing.Point(26, 156);
			this.highRankBtn.Name = "highRankBtn";
			this.highRankBtn.Size = new System.Drawing.Size(86, 23);
			this.highRankBtn.TabIndex = 1;
			this.highRankBtn.Text = "なるべく高画質";
			this.highRankBtn.UseVisualStyleBackColor = true;
			this.highRankBtn.Click += new System.EventHandler(this.highRankBtn_Click);
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.isRtmpEngine);
			this.groupBox10.Controls.Add(this.isAnotherEngineChkBox);
			this.groupBox10.Location = new System.Drawing.Point(5, 10);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(385, 67);
			this.groupBox10.TabIndex = 0;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "接続方式";
			// 
			// isRtmpEngine
			// 
			this.isRtmpEngine.Location = new System.Drawing.Point(6, 39);
			this.isRtmpEngine.Name = "isRtmpEngine";
			this.isRtmpEngine.Size = new System.Drawing.Size(252, 15);
			this.isRtmpEngine.TabIndex = 27;
			this.isRtmpEngine.Text = "RTMP（旧配信方式）";
			this.isRtmpEngine.UseVisualStyleBackColor = true;
			// 
			// isAnotherEngineChkBox
			// 
			this.isAnotherEngineChkBox.Location = new System.Drawing.Point(6, 18);
			this.isAnotherEngineChkBox.Name = "isAnotherEngineChkBox";
			this.isAnotherEngineChkBox.Size = new System.Drawing.Size(252, 15);
			this.isAnotherEngineChkBox.TabIndex = 0;
			this.isAnotherEngineChkBox.Text = "HLS（新配信方式）";
			this.isAnotherEngineChkBox.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupBox3);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
			this.tabPage2.Size = new System.Drawing.Size(403, 459);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "アカウント設定";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.cookieFileText);
			this.groupBox3.Controls.Add(this.useAccountLoginRadioBtn);
			this.groupBox3.Controls.Add(this.useCookieRadioBtn);
			this.groupBox3.Controls.Add(this.passText);
			this.groupBox3.Controls.Add(this.mailText);
			this.groupBox3.Controls.Add(this.nicoSessionComboBox1);
			this.groupBox3.Controls.Add(this.isCookieFileSiteiChkBox);
			this.groupBox3.Controls.Add(this.checkBoxShowAll);
			this.groupBox3.Controls.Add(this.loginBtn);
			this.groupBox3.Controls.Add(this.cookieFileSanshouBtn);
			this.groupBox3.Controls.Add(this.btnReload);
			this.groupBox3.Controls.Add(this.useSecondLoginChkBox);
			this.groupBox3.Location = new System.Drawing.Point(5, 10);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(385, 275);
			this.groupBox3.TabIndex = 18;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "ニコニコ動画アカウントの共有　(普段ニコニコ生放送を見ているブラウザ)";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 232);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 19);
			this.label2.TabIndex = 20;
			this.label2.Text = "パスワード：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 204);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 19);
			this.label1.TabIndex = 20;
			this.label1.Text = "メールアドレス：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// cookieFileText
			// 
			this.cookieFileText.Location = new System.Drawing.Point(20, 115);
			this.cookieFileText.Name = "cookieFileText";
			this.cookieFileText.Size = new System.Drawing.Size(297, 19);
			this.cookieFileText.TabIndex = 19;
			// 
			// useAccountLoginRadioBtn
			// 
			this.useAccountLoginRadioBtn.Checked = true;
			this.useAccountLoginRadioBtn.Location = new System.Drawing.Point(6, 183);
			this.useAccountLoginRadioBtn.Name = "useAccountLoginRadioBtn";
			this.useAccountLoginRadioBtn.Size = new System.Drawing.Size(311, 18);
			this.useAccountLoginRadioBtn.TabIndex = 18;
			this.useAccountLoginRadioBtn.TabStop = true;
			this.useAccountLoginRadioBtn.Text = "ブラウザとクッキーを共有せず、次のアカウントでログインする";
			this.useAccountLoginRadioBtn.UseVisualStyleBackColor = true;
			// 
			// useCookieRadioBtn
			// 
			this.useCookieRadioBtn.Checked = true;
			this.useCookieRadioBtn.Location = new System.Drawing.Point(6, 18);
			this.useCookieRadioBtn.Name = "useCookieRadioBtn";
			this.useCookieRadioBtn.Size = new System.Drawing.Size(189, 18);
			this.useCookieRadioBtn.TabIndex = 18;
			this.useCookieRadioBtn.TabStop = true;
			this.useCookieRadioBtn.Text = "次のブラウザとクッキーを共有する";
			this.useCookieRadioBtn.UseVisualStyleBackColor = true;
			// 
			// passText
			// 
			this.passText.Location = new System.Drawing.Point(95, 232);
			this.passText.Margin = new System.Windows.Forms.Padding(2);
			this.passText.Name = "passText";
			this.passText.Size = new System.Drawing.Size(193, 19);
			this.passText.TabIndex = 13;
			// 
			// mailText
			// 
			this.mailText.Location = new System.Drawing.Point(95, 203);
			this.mailText.Margin = new System.Windows.Forms.Padding(2);
			this.mailText.Name = "mailText";
			this.mailText.Size = new System.Drawing.Size(193, 19);
			this.mailText.TabIndex = 12;
			// 
			// nicoSessionComboBox1
			// 
			this.nicoSessionComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.nicoSessionComboBox1.Location = new System.Drawing.Point(20, 61);
			this.nicoSessionComboBox1.Margin = new System.Windows.Forms.Padding(2);
			this.nicoSessionComboBox1.Name = "nicoSessionComboBox1";
			this.nicoSessionComboBox1.Size = new System.Drawing.Size(297, 20);
			this.nicoSessionComboBox1.TabIndex = 15;
			// 
			// isCookieFileSiteiChkBox
			// 
			this.isCookieFileSiteiChkBox.AutoSize = true;
			this.isCookieFileSiteiChkBox.Location = new System.Drawing.Point(20, 94);
			this.isCookieFileSiteiChkBox.Margin = new System.Windows.Forms.Padding(2);
			this.isCookieFileSiteiChkBox.Name = "isCookieFileSiteiChkBox";
			this.isCookieFileSiteiChkBox.Size = new System.Drawing.Size(194, 16);
			this.isCookieFileSiteiChkBox.TabIndex = 16;
			this.isCookieFileSiteiChkBox.Text = "さらにクッキーファイルを直接指定する";
			this.isCookieFileSiteiChkBox.UseVisualStyleBackColor = true;
			this.isCookieFileSiteiChkBox.CheckedChanged += new System.EventHandler(this.isCookieFileSiteiChkBox_CheckedChanged);
			// 
			// checkBoxShowAll
			// 
			this.checkBoxShowAll.AutoSize = true;
			this.checkBoxShowAll.Location = new System.Drawing.Point(20, 41);
			this.checkBoxShowAll.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxShowAll.Name = "checkBoxShowAll";
			this.checkBoxShowAll.Size = new System.Drawing.Size(151, 16);
			this.checkBoxShowAll.TabIndex = 16;
			this.checkBoxShowAll.Text = "すべてのブラウザを表示する";
			this.checkBoxShowAll.UseVisualStyleBackColor = true;
			this.checkBoxShowAll.CheckedChanged += new System.EventHandler(this.checkBoxShowAll_CheckedChanged);
			// 
			// loginBtn
			// 
			this.loginBtn.Location = new System.Drawing.Point(302, 230);
			this.loginBtn.Margin = new System.Windows.Forms.Padding(2);
			this.loginBtn.Name = "loginBtn";
			this.loginBtn.Size = new System.Drawing.Size(69, 23);
			this.loginBtn.TabIndex = 17;
			this.loginBtn.Text = "ログインする";
			this.loginBtn.UseVisualStyleBackColor = true;
			this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
			// 
			// cookieFileSanshouBtn
			// 
			this.cookieFileSanshouBtn.Location = new System.Drawing.Point(322, 113);
			this.cookieFileSanshouBtn.Margin = new System.Windows.Forms.Padding(2);
			this.cookieFileSanshouBtn.Name = "cookieFileSanshouBtn";
			this.cookieFileSanshouBtn.Size = new System.Drawing.Size(40, 23);
			this.cookieFileSanshouBtn.TabIndex = 17;
			this.cookieFileSanshouBtn.Text = "参照";
			this.cookieFileSanshouBtn.UseVisualStyleBackColor = true;
			this.cookieFileSanshouBtn.Click += new System.EventHandler(this.cookieFileSiteiSanshouBtn_Click);
			// 
			// btnReload
			// 
			this.btnReload.Location = new System.Drawing.Point(322, 59);
			this.btnReload.Margin = new System.Windows.Forms.Padding(2);
			this.btnReload.Name = "btnReload";
			this.btnReload.Size = new System.Drawing.Size(40, 23);
			this.btnReload.TabIndex = 17;
			this.btnReload.Text = "更新";
			this.btnReload.UseVisualStyleBackColor = true;
			this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
			// 
			// useSecondLoginChkBox
			// 
			this.useSecondLoginChkBox.Location = new System.Drawing.Point(20, 139);
			this.useSecondLoginChkBox.Margin = new System.Windows.Forms.Padding(2);
			this.useSecondLoginChkBox.Name = "useSecondLoginChkBox";
			this.useSecondLoginChkBox.Size = new System.Drawing.Size(374, 36);
			this.useSecondLoginChkBox.TabIndex = 20;
			this.useSecondLoginChkBox.Text = "ブラウザからクッキーが取得できなかった場合、次のアカウントでログインする";
			this.useSecondLoginChkBox.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(249, 500);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(74, 23);
			this.button3.TabIndex = 1;
			this.button3.Text = "OK";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.optionOk_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(329, 500);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(74, 23);
			this.button4.TabIndex = 1;
			this.button4.Text = "キャンセル";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.optionCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.latencyList);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Location = new System.Drawing.Point(5, 289);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(385, 46);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "遅延";
			// 
			// latencyList
			// 
			this.latencyList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.latencyList.FormattingEnabled = true;
			this.latencyList.Items.AddRange(new object[] {
									"0.5",
									"1.0",
									"1.5",
									"3.0"});
			this.latencyList.Location = new System.Drawing.Point(138, 15);
			this.latencyList.Name = "latencyList";
			this.latencyList.Size = new System.Drawing.Size(44, 20);
			this.latencyList.TabIndex = 35;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 18);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(140, 19);
			this.label14.TabIndex = 34;
			this.label14.Text = "HLS接続時の遅延レベル：";
			// 
			// optionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(423, 545);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.tabControl1);
			this.Location = new System.Drawing.Point(600, 100);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.Name = "optionForm";
			this.Text = "オプション";
			this.tabControl1.ResumeLayout(false);
			this.tabPage6.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label label14;
		public System.Windows.Forms.ComboBox latencyList;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton isRtmpEngine;
		private System.Windows.Forms.Button anotherPlayerSanshouBtn;
		private System.Windows.Forms.TextBox anotherPlayerPathText;
		private System.Windows.Forms.GroupBox groupBox8;
		//private System.Windows.Forms.Button anotherCommentViewerSanshouBtn;
		//private System.Windows.Forms.TextBox anotherCommentViewerPathText;
		//private System.Windows.Forms.RadioButton isDefaultCommentViewerRadioBtn;
		//private System.Windows.Forms.RadioButton isAnotherCommentViewerRadioBtn;
		private System.Windows.Forms.RadioButton isAnotherEngineChkBox;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.Button upBtn;
		private System.Windows.Forms.Button downBtn;
		private System.Windows.Forms.ListBox qualityListBox;
		private System.Windows.Forms.Button lowRankBtn;
		private System.Windows.Forms.Button highRankBtn;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.CheckBox useSecondLoginChkBox;
		private System.Windows.Forms.Button loginBtn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cookieFileSanshouBtn;
		private System.Windows.Forms.CheckBox isCookieFileSiteiChkBox;
		private System.Windows.Forms.RadioButton useAccountLoginRadioBtn;
		private System.Windows.Forms.TextBox cookieFileText;
		private System.Windows.Forms.RadioButton useCookieRadioBtn;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button btnReload;
		private System.Windows.Forms.CheckBox checkBoxShowAll;
		//private SunokoLibrary.Windows.Forms.NicoSessionComboBox nicoSessionComboBox1;
		private namaichi.NicoSessionComboBox2 nicoSessionComboBox1;
		private System.Windows.Forms.TextBox mailText;
		private System.Windows.Forms.TextBox passText;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabControl tabControl1;
		

		

	}
}
