// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Tool.GDBDebugger.GDB
{
	public class VMState
	{
		public const int DefaultConnectionPort = 1234;

		public TcpClient TcpClient { get; set; }
		public GDBClient GDBClient { get; set; }

		public bool Connect(int port = DefaultConnectionPort)
		{
			Disconnect();

			TcpClient = new TcpClient("localhost", port);
			var stream = new GDBNetworkStream(TcpClient.Client, true);
			GDBClient = new GDBClient(stream);

			return GDBClient.IsConnected;
		}

		public void Disconnect()
		{
			if (GDBClient == null)
				return;

			TcpClient.Close();

			TcpClient = null;
			GDBClient = null;
		}

		public void Restart()
		{ }

		public void Step()
		{ }

		public void Continue()
		{ }

		public void Break()
		{ }
	}
}
