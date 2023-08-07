// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.HelloWorld.ARMv8A32;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");
		Debug.WriteLine("MOSA ARMv8A32 Kernel");

		Program.EntryPoint();
	}
}
