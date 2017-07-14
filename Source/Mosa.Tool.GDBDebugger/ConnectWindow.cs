// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger
{
	public partial class ConnectWindow : Form
	{
		public Options Options { get; set; }

		public ConnectWindow()
		{
			InitializeComponent();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			Options.GDBHost = tbHost.Text;
			Options.GDBPort = (int)numPort.Value;

			DialogResult = DialogResult.OK;
		}

		private void CheckConnectButton(object sender, EventArgs e)
		{
			btnConnect.Enabled = (!string.IsNullOrEmpty(tbHost.Text));
		}
	}
}
