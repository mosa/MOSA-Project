// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;
using Mosa.Kernel.x86;

namespace Mosa.Application
{
	/// <summary>
	/// Mem
	/// </summary>
	public class Mem : BaseApplication, IConsoleApp
	{
		public override int Start(string parameters)
		{
			Console.WriteLine("*** Memory ****");
			Console.Write(" Total Pages : ");
			Console.WriteLine(PageFrameAllocator.TotalPages.ToString());
			Console.Write(" Used Pages  : ");
			Console.WriteLine(PageFrameAllocator.TotalPagesInUse.ToString());
			Console.Write(" Page Size   : ");
			Console.WriteLine(PageFrameAllocator.PageSize.ToString());
			Console.Write(" Free Memory : ");
			Console.Write(((PageFrameAllocator.TotalPages - PageFrameAllocator.TotalPagesInUse) * PageFrameAllocator.PageSize / (1024 * 1024)).ToString());
			Console.WriteLine(" MB");

			return 0;
		}
	}
}
