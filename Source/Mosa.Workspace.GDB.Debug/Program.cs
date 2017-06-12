// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using Mosa.Utility.RSP.Command;
using System;
using System.Net.Sockets;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var tcpClient = new TcpClient("localhost", 2345);

			var gdbClient = new GDBClient();
			gdbClient.Stream = new GDBNetworkStream(tcpClient.Client, true);

			gdbClient.SendCommandAsync(new GeneralRegisterRead(Response));
			gdbClient.SendCommandAsync(new GeneralRegisterRead(Response));
			gdbClient.SendCommandAsync(new GeneralRegisterRead(Response));
			gdbClient.SendCommandAsync(new Continue(Response));

			while (true)
			{
				//loop
			}

			return;
		}

		private static void Response(BaseCommand command)
		{
			Console.WriteLine(command.Command);

			return;
		}
	}
}
