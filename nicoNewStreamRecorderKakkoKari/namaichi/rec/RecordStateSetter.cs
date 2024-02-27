/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/05/26
 * Time: 19:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using namaichi.info;
using namaichi.utility;

namespace namaichi.rec
{
	/// <summary>
	/// Description of RecordStateSetter.
	/// </summary>
	public class RecordStateSetter
	{
		private MainForm form;
		public string samuneUrl;
		private bool isChannelPlus = false;
			
		public RecordStateSetter(MainForm form, bool isChannelPlus = false)
		{
			this.form = form;
			this.isChannelPlus = isChannelPlus;
		}
		public void set(string data, string res) {
			setInfo(data, form);
			Task.Run(() => setSamune(data, form));
		}
		private void setInfo(string data, MainForm form) {
			samuneUrl = util.getRegGroup(data, "\"thumbnailImageUrl\":\"(.+?)\"");
			if (samuneUrl == null) samuneUrl = util.getRegGroup(data, "\"small\":\"(.+?)\"");
			if (samuneUrl == null) samuneUrl = util.getRegGroup(data, "thumbnail:.+?'(https*://.+?)'");
			if (samuneUrl == null) samuneUrl = util.getRegGroup(data, "<thumb_url>(.+?)</thumb_url>");
			if (samuneUrl == null) samuneUrl = util.getRegGroup(data, "\"thumbnail_url\":\"(.+?)\"");
		}
		private void setSamune(string data, MainForm form) {
			form.setSamune(samuneUrl);
		}
	}
}
