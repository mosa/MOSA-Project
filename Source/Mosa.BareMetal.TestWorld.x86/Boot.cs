// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.UnitTests.Optimization;

namespace Mosa.BareMetal.TestWorld.x86;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");
		Debug.WriteLine("MOSA x86 Kernel");

		Division.DivisionBy7(254u);

		Program.EntryPoint();
	}
}
