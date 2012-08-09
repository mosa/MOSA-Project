/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.IO.Pipes;
using System.Net;
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

		public override void Connect()
		{
			this.btnConnect.Enabled = false;
			this.btnDisconnect.Enabled = true;
		}

		public override void Disconnect()
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
				switch (comboBox1.SelectedIndex)
				{
					case 0:
						var client = new TcpClient(tbServerName.Text.Trim(), Convert.ToInt32(tbPort.Text));
						DebugEngine.Stream = new DebugNetworkStream(client.Client, true);
						break;		
					case 2:
						var pipeStream = new NamedPipeClientStream(".", tbNamedPipe.Text.Trim(), PipeDirection.InOut);
						pipeStream.Connect();
						DebugEngine.Stream = pipeStream;
						break;
					//case 2:
					//    var server = new TcpListener(Convert.ToInt32(tbPort.Text));
					//    DebugEngine.Stream = new DebugNetworkStream(server.AcceptSocket(), true);
					//    break;
					default:
						Status = "Connection method not supported yet!";
						break;
				}

				if (DebugEngine.IsConnected)
				{
					Status = "Connected!";
					MainForm.SignalConnect();
				}

			}
			catch (Exception ex)
			{
				Status = "ERROR: " + ex.Message;
			}
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			var stream = DebugEngine.Stream;
			if (stream != null)
			{
				DebugEngine.Stream = null;
				stream.Close();
				MainForm.SignalDisconnect();
			}
		}

	}
}