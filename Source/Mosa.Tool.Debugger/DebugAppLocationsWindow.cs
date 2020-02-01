// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Launcher;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger
{
	public partial class DebugAppLocationsWindow : Form
	{
		private readonly MainForm MainForm;

		public DebugAppLocationsWindow(MainForm mainForm)
		{
			InitializeComponent();

			MainForm = mainForm;
		}

		private void DebugQemuWindow_Load(object sender, EventArgs e)
		{
			tbQEMU.Text = MainForm.QEMU;
			tbBIOSDirectory.Text = MainForm.QEMUBios;
		}

		private void btnDebug_Click(object sender, EventArgs e)
		{
			MainForm.QEMU = tbQEMU.Text;
			MainForm.QEMUBios = tbBIOSDirectory.Text;

			Close();
		}
	}
}
