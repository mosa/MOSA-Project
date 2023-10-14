// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		tbQEMU.Text = MainForm.MosaSettings.QemuX86App;
		tbBIOSDirectory.Text = MainForm.MosaSettings.QemuBIOS;
	}

	private void btnDebug_Click(object sender, EventArgs e)
	{
		MainForm.MosaSettings.QemuX86App = tbQEMU.Text;
		MainForm.MosaSettings.QemuBIOS = tbBIOSDirectory.Text;

		Close();
	}
}
