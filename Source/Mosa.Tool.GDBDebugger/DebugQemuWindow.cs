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
	public partial class DebugQemuWindow : Form
	{
		private readonly AppLocations AppLocations;
		private readonly Options Options;
		private ImageFormat ImageFormat;

		public Process QEMUProcess
		{
			get;
			private set;
		}

		public DebugQemuWindow(AppLocations apps, Options options)
		{
			InitializeComponent();

			Options = options;
			AppLocations = apps;
		}

		private void DebugQemuWindow_Load(object sender, EventArgs e)
		{
			tbQEMU.Text = AppLocations.QEMU;
			tbBIOSDirectory.Text = AppLocations.QEMUBIOSDirectory;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnDebug_Click(object sender, EventArgs e)
		{
			if (LaunchAndDebug())
			{
				DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.Cancel;
			}
		}

		private void btnImageBrowse_Click(object sender, EventArgs e)
		{
			using (var imageDialog = new OpenFileDialog())
			{
				imageDialog.Filter = "Supported files (*.iso,*.img)|*.iso;*.img|IMG files (*.img)|*.img|ISO files (*.iso)|*.iso";

				if (imageDialog.ShowDialog(this) == DialogResult.OK)
				{
					tbImage.Text = imageDialog.FileName;

					ImageFormat = GetFormat(tbImage.Text);
				}
			}
		}

		private ImageFormat GetFormat(string fileName)
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
			btnDebug.Enabled = (!string.IsNullOrEmpty(tbImage.Text) && File.Exists(tbImage.Text) &&
								!string.IsNullOrEmpty(tbQEMU.Text) && File.Exists(tbQEMU.Text) &&
								!string.IsNullOrEmpty(tbBIOSDirectory.Text) && Directory.Exists(tbBIOSDirectory.Text));
		}

		private string ResolvePath(string path)
		{
			if (Path.IsPathRooted(path))
				return path;

			return Path.Combine(Application.StartupPath, path);
		}

		private bool LaunchAndDebug()
		{
			QEMUProcess = StartQEMU(Options.GDBPort);

			return true;
		}

		private Process StartQEMU(int debugPort)
		{
			var info = new ProcessStartInfo();
			info.FileName = tbQEMU.Text;

			info.Arguments = " -L \"" + tbBIOSDirectory.Text + "\"";

			//TODO: Check platform
			info.Arguments += " -cpu qemu32,+sse4.1";

			info.Arguments = info.Arguments + " -S -gdb tcp::" + debugPort.ToString();

			if (ImageFormat == ImageFormat.ISO)
			{
				info.Arguments = info.Arguments + " -cdrom \"" + tbImage.Text + "\"";
			}
			else
			{
				info.Arguments = info.Arguments + " -hda \"" + tbImage.Text + "\"";
			}

			info.UseShellExecute = false;
			info.CreateNoWindow = true;

			return Process.Start(info);
		}
	}
}
