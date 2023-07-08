// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger;

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
		tbQEMU.Text = MainForm.MosaSettings.QEMUApp;
		tbBIOSDirectory.Text = MainForm.MosaSettings.QEMUBios;
	}

	private void btnDebug_Click(object sender, EventArgs e)
	{
		MainForm.MosaSettings.QEMUApp = tbQEMU.Text;
		MainForm.MosaSettings.QEMUBios = tbBIOSDirectory.Text;

		Close();
	}
}
