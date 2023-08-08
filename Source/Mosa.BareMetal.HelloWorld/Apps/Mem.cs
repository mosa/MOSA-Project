// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Mem : IApp
{
	public string Name => "Mem";

	public string Description => "Shows memory information.";

	public void Execute()
	{
		Console.WriteLine("**** Memory ****");
		Console.WriteLine(" Total Pages : " + PageFrameAllocator.TotalPages);
		Console.WriteLine(" Used Pages  : " + PageFrameAllocator.UsedPages);
		Console.WriteLine(" Page Size   : " + Page.Size);
		Console.WriteLine(" Free Memory : " + (PageFrameAllocator.TotalPages - PageFrameAllocator.UsedPages) * Page.Size / (1024 * 1024) + " MB");
	}
}
