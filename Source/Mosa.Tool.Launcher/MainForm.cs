using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : Form
	{
		public string SourceFile { get; set; }
		public string DestinationDirectory { get; set; }

		public MainForm()
		{
			InitializeComponent();
		}

		private void btnSource_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceFile = openFileDialog1.FileName;
				lbSource.Text = Path.GetFileName(SourceFile);
				lbSourceDirectory.Text = Path.GetDirectoryName(SourceFile);

				if (String.IsNullOrWhiteSpace(DestinationDirectory))
				{
					DestinationDirectory = Path.GetDirectoryName(SourceFile) + Path.DirectorySeparatorChar + "build";
					lbDestinationDirectory.Text = DestinationDirectory;
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			cbPlatform.SelectedIndex = 0;
			cbImageFormat.SelectedIndex = 0;
			cbLinkerFormat.SelectedIndex = 0;
		}
	}
}
