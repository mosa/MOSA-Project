// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class DumpInfo : IApp
{
	public string Name => "DumpInfo";

	public string Description => "Dumps information about the system.";

	public void Execute()
	{
		var firmware = Platform.GetFirmware();
		var cpu = Platform.GetCPU();

		Console.WriteLine("[Firmware]");
		Console.WriteLine("Vendor  : " + firmware.Vendor);
		Console.WriteLine("Version : " + firmware.Version);
		Console.WriteLine("Date    : " + firmware.Date);
		Console.WriteLine();
		Console.WriteLine("[CPU]");
		Console.WriteLine("Cores   : " + cpu.Cores);
		Console.WriteLine("Vendor  : " + cpu.Vendor);
		Console.WriteLine("Model   : " + cpu.Model);
	}
}
