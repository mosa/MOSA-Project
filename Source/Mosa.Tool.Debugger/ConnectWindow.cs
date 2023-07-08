// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;
using Mosa.Compiler.Common.Configuration;
using Mosa.Utility.Configuration;

namespace Mosa.Tool.Debugger;

public partial class ConnectWindow : Form
{
	private readonly MosaSettings MosaSettings;

	public ConnectWindow(MosaSettings settings)
	{
		InitializeComponent();
		MosaSettings = settings;
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
	}

	private void btnConnect_Click(object sender, EventArgs e)
	{
		MosaSettings.GDBHost = tbHost.Text;
		MosaSettings.GDBPort = (int)numPort.Value;

		DialogResult = DialogResult.OK;
	}

	private void CheckConnectButton(object sender, EventArgs e)
	{
		btnConnect.Enabled = (!string.IsNullOrEmpty(tbHost.Text));
	}
}
