// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Net.Sockets;
using Mosa.Utility.RSP;
using Mosa.Utility.RSP.Command;

namespace Mosa.Workspace.UnitTest.Debug;

internal class Program
{
	private static void Main()
	{
		var tcpClient = new TcpClient("localhost", 2345);

		var gdbClient = new GDBClient();
		gdbClient.Stream = new GDBNetworkStream(tcpClient.Client, true);

		gdbClient.SendCommand(new ReadMemory(0x0, 2, ReadMemory));

		//gdbClient.SendCommandAsync(new SetSoftwareBreakPoint(0x400030, 1, Response));
		gdbClient.SendCommand(new SetSoftwareBreakPoint(0x4571B9, 1, Response));
		gdbClient.SendCommand(new GetRegisters(GetRegisters));
		gdbClient.SendCommand(new Continue(Response));

		Thread.Sleep(2000);
		gdbClient.SendBreak();
		gdbClient.SendCommand(new GetRegisters(GetRegisters));
		gdbClient.SendCommand(new Continue(Response));

		Thread.Sleep(2000);
		gdbClient.SendBreak();
		gdbClient.SendCommand(new GetRegisters(GetRegisters));

		//0x400030
		//gdbClient.SendCommandAsync(new GeneralRegisterRead(Response));

		while (true)
		{
			//loop
		}
	}

	private static void Response(GDBCommand command)
	{
		Console.WriteLine("COMMAND: " + command.Pack);
		Console.WriteLine("RESPONSE: " + command.ResponseAsString);

		return;
	}

	private static void ReadMemory(GDBCommand command)
	{
		Console.WriteLine("COMMAND: " + command.Pack);
		Console.WriteLine("RESPONSE: " + command.ResponseAsString);

		var rm = command as ReadMemory;

		return;
	}

	private static void ReadRegister(GDBCommand command)
	{
		Console.WriteLine("COMMAND: " + command.Pack);
		Console.WriteLine("RESPONSE: " + command.ResponseAsString);

		var rr = command as ReadRegister;

		return;
	}

	private static void GetRegisters(GDBCommand command)
	{
		Console.WriteLine("COMMAND: " + command.Pack);
		Console.WriteLine("RESPONSE: " + command.ResponseAsString);

		var rr = command as GetRegisters;

		return;
	}
}
