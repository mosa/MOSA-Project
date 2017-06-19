// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using Mosa.Utility.RSP.Command;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var tcpClient = new TcpClient("localhost", 2345);

			var gdbClient = new GDBClient();
			gdbClient.Stream = new GDBNetworkStream(tcpClient.Client, true);

			gdbClient.SendCommandAsync(new ReadMemory(0x0, 2, ReadMemory));

			//gdbClient.SendCommandAsync(new SetSoftwareBreakPoint(0x400030, 1, Response));
			gdbClient.SendCommandAsync(new SetSoftwareBreakPoint(0x4571B9, 1, Response));
			gdbClient.SendCommandAsync(new GetRegisters(GetRegisters));
			gdbClient.SendCommandAsync(new Continue(Response));

			Thread.Sleep(2000);
			gdbClient.SendBreak();
			gdbClient.SendCommandAsync(new GetRegisters(GetRegisters));
			gdbClient.SendCommandAsync(new Continue(Response));

			Thread.Sleep(2000);
			gdbClient.SendBreak();
			gdbClient.SendCommandAsync(new GetRegisters(GetRegisters));

			//0x400030
			//gdbClient.SendCommandAsync(new GeneralRegisterRead(Response));

			while (true)
			{
				//loop
			}

			return;
		}

		private static void Response(BaseCommand command)
		{
			Console.WriteLine("COMMAND: " + command.Pack);
			Console.WriteLine("RESPONSE: " + command.ResponseAsString);

			return;
		}

		private static void ReadMemory(BaseCommand command)
		{
			Console.WriteLine("COMMAND: " + command.Pack);
			Console.WriteLine("RESPONSE: " + command.ResponseAsString);

			var rm = command as ReadMemory;

			return;
		}

		private static void ReadRegister(BaseCommand command)
		{
			Console.WriteLine("COMMAND: " + command.Pack);
			Console.WriteLine("RESPONSE: " + command.ResponseAsString);

			var rr = command as ReadRegister;

			return;
		}

		private static void GetRegisters(BaseCommand command)
		{
			Console.WriteLine("COMMAND: " + command.Pack);
			Console.WriteLine("RESPONSE: " + command.ResponseAsString);

			var rr = command as GetRegisters;

			return;
		}
	}
}
