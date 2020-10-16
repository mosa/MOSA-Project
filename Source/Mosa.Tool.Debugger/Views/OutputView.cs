// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class OutputView : DebugDockContent
	{
		public OutputView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public void AddOutput(string data)
		{
			richTextBox1.AppendText(data);
			richTextBox1.AppendText("\n");
			richTextBox1.Update();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			richTextBox1.Clear();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			if (string.IsNullOrEmpty(saveFileDialog1.FileName))
				return;

			richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
		}
	}
}
