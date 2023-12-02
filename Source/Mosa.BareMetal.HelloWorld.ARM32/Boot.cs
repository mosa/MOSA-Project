// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.HelloWorld.ARM32;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");
		Debug.WriteLine("MOSA ARM32 Kernel");

		Program.EntryPoint();
	}

	public static void ForceInclude()
	{
		Mosa.Kernel.BareMetal.ARM32.PlatformPlug.ForceInclude();
	}
}
