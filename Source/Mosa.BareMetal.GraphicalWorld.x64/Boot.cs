// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.GraphicalWorld.x64;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");
		Debug.WriteLine("MOSA x64 Kernel");

		Program.EntryPoint();
	}

	public static void Include()
	{
		Kernel.BareMetal.x64.VGAText.Clear();
	}
}
