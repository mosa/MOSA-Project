// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.UnitTests.Basic;

namespace Mosa.BareMetal.HelloWorld.x86;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");
		Debug.WriteLine("MOSA x86 Kernel");

		Program.EntryPoint();
	}

	public static void Include()
	{
		Kernel.BareMetal.x86.Scheduler.SwitchToThread(null);
	}
}
