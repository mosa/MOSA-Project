// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.DebugEngine;
using System;
using System.IO.Pipes;
using System.Net.Sockets;

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
			tbPort.Enabled = (comboBox1.SelectedIndex != 2);
			tbNamedPipe.Visible = (comboBox1.SelectedIndex == 2);
			tbServerName.Visible = (comboBox1.SelectedIndex != 2);

			lbPipeName.Visible = (comboBox1.SelectedIndex == 2);
			lbServerName.Visible = (comboBox1.SelectedIndex != 2);
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

					//case 1:
					//	var server = new TcpListener(System.Net.IPAddress.Parse(tbServerName.Text.Trim()), Convert.ToInt32(tbPort.Text));
					//	var helper = new TcpServer(DebugEngine, server);
					//	helper.Start();
					//	break;

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

		private class TcpServer
		{
			private TcpListener listener;
			private DebugServerEngine debugEngine;

			public TcpServer(DebugServerEngine debugEngine, TcpListener listener)
			{
				this.debugEngine = debugEngine;
				this.listener = listener;
			}

			private IAsyncResult result;
			public void Start()
			{
				listener.Start();
				result = listener.BeginAcceptSocket(onBeginAccept, null);
			}

			public void Stop()
			{
				listener.Stop();
			}

			private void onBeginAccept(object state)
			{
				var socket = listener.EndAcceptSocket(result);
				debugEngine.Stream = new DebugNetworkStream(socket, true);
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