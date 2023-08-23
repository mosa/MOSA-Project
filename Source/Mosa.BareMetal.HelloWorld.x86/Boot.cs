// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.UnitTests.Primitive;

namespace Mosa.BareMetal.HelloWorld.x86;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");
		Debug.WriteLine("MOSA x86 Kernel");

		var result = CheckedTests.AddU8U8(18446744073709551615, 1);

		if (result == 95272687)
		{
			Debug.WriteLine("CheckedTests::AddU8U8() -> Pass");
		}
		else
		{
			Debug.WriteLine("CheckedTests::AddU8U8() -> Fail");
		}

		Debug.WriteLine("##PASS##");

		while (true)
		{
		}
	}

	public static void Include()
	{
		Kernel.BareMetal.x86.Scheduler.SwitchToThread(null);
	}
}
