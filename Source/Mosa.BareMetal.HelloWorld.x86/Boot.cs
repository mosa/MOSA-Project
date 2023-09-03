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

		var result = ArrayLayoutTests.C_4();

		if (result)
		{
			Debug.WriteLine("ArrayLayoutTests.C_4() -> Pass");
		}
		else
		{
			Debug.WriteLine("ArrayLayoutTests.C_4() -> Fail");
		}

		var result2 = ArrayLayoutTests.C_4a();

		if (result2)
		{
			Debug.WriteLine("ArrayLayoutTests.C_4a() -> Pass");
		}
		else
		{
			Debug.WriteLine("ArrayLayoutTests.C_4a() -> Fail");
		}

		var result3 = ArrayLayoutTests.C_4z();

		if (result3)
		{
			Debug.WriteLine("ArrayLayoutTests.C_4z() -> Pass");
		}
		else
		{
			Debug.WriteLine("ArrayLayoutTests.C_4z() -> Fail");
		}

		//var result4 = ArrayLayoutTests.C_4ab();
		//Debug.WriteLine("ArrayLayoutTests.C_4ab() = ", result4);

		//var result = CheckedTests.AddU8U8(18446744073709551615, 1);

		//if (result == 95272687)
		//{
		//	Debug.WriteLine("CheckedTests::AddU8U8() -> Pass");
		//}
		//else
		//{
		//	Debug.WriteLine("CheckedTests::AddU8U8() -> Fail");
		//}

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
