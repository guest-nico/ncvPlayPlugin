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
			this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.バージョン情報VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label12 = new System.Windows.Forms.Label();
			this.playerBtn = new System.Windows.Forms.Button();
			this.logText = new System.Windows.Forms.TextBox();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// qualityBox
			// 
			this.qualityBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.qualityBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.qualityBox.FormattingEnabled = true;
			this.qualityBox.Location = new System.Drawing.Point(385, 38);
			this.qualityBox.Name = "qualityBox";
			this.qualityBox.Size = new System.Drawing.Size(79, 20);
			this.qualityBox.TabIndex = 19;
			this.qualityBox.TextChanged += new System.EventHandler(this.QualityBoxTextUpdate);
			// 
			// urlText
			// 
			this.urlText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.urlText.Location = new System.Drawing.Point(69, 38);
			this.urlText.Margin = new System.Windows.Forms.Padding(2);
			this.urlText.Name = "urlText";
			this.urlText.Size = new System.Drawing.Size(241, 19);
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
									this.helpMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(476, 26);
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
			this.playerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.playerBtn.Location = new System.Drawing.Point(314, 35);
			this.playerBtn.Name = "playerBtn";
			this.playerBtn.Size = new System.Drawing.Size(64, 24);
			this.playerBtn.TabIndex = 1;
			this.playerBtn.Text = "視聴";
			this.playerBtn.UseVisualStyleBackColor = true;
			this.playerBtn.Click += new System.EventHandler(this.PlayerBtnClick);
			// 
			// logText
			// 
			this.logText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.logText.Location = new System.Drawing.Point(6, 71);
			this.logText.Margin = new System.Windows.Forms.Padding(2);
			this.logText.Multiline = true;
			this.logText.Name = "logText";
			this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logText.Size = new System.Drawing.Size(459, 81);
			this.logText.TabIndex = 5;
			this.logText.TabStop = false;
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(476, 163);
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
			this.Name = "MainForm";
			this.Text = "ニコ生新配信録画ツール（仮 ver0.86.15";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_Close);
			this.Load += new System.EventHandler(this.mainForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ComboBox qualityBox;
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
