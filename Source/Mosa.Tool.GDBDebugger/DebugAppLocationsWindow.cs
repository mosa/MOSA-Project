// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Launcher;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger
{
	public partial class DebugAppLocationsWindow : Form
	{
		private readonly AppLocations AppLocations;

		public DebugAppLocationsWindow(AppLocations apps)
		{
			InitializeComponent();

			AppLocations = apps;
		}

		private void DebugQemuWindow_Load(object sender, EventArgs e)
		{
			tbQEMU.Text = AppLocations.QEMU;
			tbBIOSDirectory.Text = AppLocations.QEMUBIOSDirectory;
		}

		private void btnDebug_Click(object sender, EventArgs e)
		{
			AppLocations.QEMU = tbQEMU.Text;
			AppLocations.QEMUBIOSDirectory = tbBIOSDirectory.Text;

			Close();
		}
	}
}
