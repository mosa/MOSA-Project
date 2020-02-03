// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using Mosa.Utility.BootImage;
using Mosa.Utility.Launcher;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger
{
	public partial class DebugVMImageWindow : Form
	{
		private readonly AppLocations AppLocations;
		private readonly Options Options;
		private ImageFormat ImageFormat;

		public DebugVMImageWindow(AppLocations apps, Options options)
		{
			InitializeComponent();

			Options = options;
			AppLocations = apps;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnDebug_Click(object sender, EventArgs e)
		{
			StartQEMU(Options.GDBPort);

			DialogResult = DialogResult.OK;
		}

		private void btnImageBrowse_Click(object sender, EventArgs e)
		{
			using (var imageDialog = new OpenFileDialog())
			{
				imageDialog.Filter = "Supported files (*.iso,*.img)|*.iso;*.img|IMG files (*.img)|*.img|ISO files (*.iso)|*.iso";

				if (imageDialog.ShowDialog(this) == DialogResult.OK)
				{
					tbImageFile.Text = imageDialog.FileName;

					ImageFormat = GetFormat(tbImageFile.Text);
				}
			}
		}

		private static ImageFormat GetFormat(string fileName)
		{
			string extension = Path.GetExtension(fileName);

			switch (extension.ToLower())
			{
				case ".img":
					return ImageFormat.IMG;

				case ".iso":
					return ImageFormat.ISO;
			}

			return ImageFormat.NotSpecified;
		}

		private void CheckDebugButton(object sender, EventArgs e)
		{
			btnDebug.Enabled = !string.IsNullOrEmpty(tbImageFile.Text) && File.Exists(tbImageFile.Text);
		}

		private string ResolvePath(string path)
		{
			if (Path.IsPathRooted(path))
				return path;

			return Path.Combine(Application.StartupPath, path);
		}

		private Process StartQEMU(int debugPort)
		{
			var info = new ProcessStartInfo();
			info.FileName = AppLocations.QEMU;

			info.Arguments = " -L \"" + AppLocations.QEMUBIOSDirectory + "\"";

			//TODO: Check platform
			info.Arguments += " -cpu qemu32,+sse4.1";

			info.Arguments = info.Arguments + " -S -gdb tcp::" + debugPort.ToString();

			if (ImageFormat == ImageFormat.ISO)
			{
				info.Arguments = info.Arguments + " -cdrom \"" + tbImageFile.Text + "\"";
			}
			else
			{
				info.Arguments = info.Arguments + " -hda \"" + tbImageFile.Text + "\"";
			}

			info.UseShellExecute = false;
			info.CreateNoWindow = true;

			return Process.Start(info);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
		}
	}
}
