// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;
using Mosa.Compiler.Common.Configuration;

namespace Mosa.Tool.Debugger
{
	public partial class ConnectWindow : Form
	{
		private readonly Settings Settings;

		public ConnectWindow(Settings settings)
		{
			InitializeComponent();
			Settings = settings;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			Settings.SetValue("GDB.Host", tbHost.Text);
			Settings.SetValue("GDB.Port", (int)numPort.Value);

			DialogResult = DialogResult.OK;
		}

		private void CheckConnectButton(object sender, EventArgs e)
		{
			btnConnect.Enabled = (!string.IsNullOrEmpty(tbHost.Text));
		}
	}
}
