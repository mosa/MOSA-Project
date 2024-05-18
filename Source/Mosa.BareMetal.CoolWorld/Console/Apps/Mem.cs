// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class Mem : IApp
{
	public string Name => "Mem";

	public string Description => "Shows memory information.";

	public void Execute()
	{
		System.Console.WriteLine("**** Memory ****");
		System.Console.WriteLine(" Total Pages : " + PageFrameAllocator.TotalPages);
		System.Console.WriteLine(" Used Pages  : " + PageFrameAllocator.UsedPages);
		System.Console.WriteLine(" Page Size   : " + Page.Size);
		System.Console.WriteLine(" Free Memory : " + (PageFrameAllocator.TotalPages - PageFrameAllocator.UsedPages) * Page.Size / (1024 * 1024) + " MB");
	}
}
