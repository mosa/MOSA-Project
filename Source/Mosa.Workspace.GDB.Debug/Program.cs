// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using System.Net.Sockets;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var tcpClient = new TcpClient("localhost", 1234);

			var gdbClient = new GDBClient();
			gdbClient.Stream = new GDBNetworkStream(tcpClient.Client, true);
		}
	}
}
