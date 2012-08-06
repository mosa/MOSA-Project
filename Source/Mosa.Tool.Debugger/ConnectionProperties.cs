using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Windows.Forms;
using Mosa.Utility.DebugEngine;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Debugger
{
	public partial class ConnectionProperties : DebuggerDockContent
	{
		public ConnectionProperties()
		{
			InitializeComponent();
		}

		public override void OnConnect()
		{
			this.btnConnect.Enabled = false;
			this.btnDisconnect.Enabled = true;
		}

		public override void OnDisconnect()
		{
			this.btnConnect.Enabled = true;
			this.btnDisconnect.Enabled = false;
		}

		private void ConnectionProperties_Load(object sender, EventArgs e)
		{
			comboBox1.SelectedIndex = 0;
			comboBox1_SelectedIndexChanged(sender, e);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			tbPort.Enabled = (comboBox1.SelectedIndex != 0);
			tbNamedPipe.Visible = (comboBox1.SelectedIndex == 0);
			tbServerName.Visible = (comboBox1.SelectedIndex != 0);

			lbPipeName.Visible = (comboBox1.SelectedIndex == 0);
			lbServerName.Visible = (comboBox1.SelectedIndex != 0);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DebugEngine.Stream = null;

			Status = "Attempting to connect...";
			try
			{
				if (comboBox1.SelectedIndex == 0)
				{
					var pipeStream = new NamedPipeClientStream(".", tbNamedPipe.Text.Trim(), PipeDirection.InOut);
					pipeStream.Connect();
					DebugEngine.Stream = pipeStream;
				}
				else
				{
					Status = "Connection method not supported yet!";
				}

				if (DebugEngine.IsConnected)
				{
					Status = "Connected!";
				}

			}
			catch (Exception ex)
			{
				Status = "ERROR: " + ex.Message;
			}
		}

	}
}