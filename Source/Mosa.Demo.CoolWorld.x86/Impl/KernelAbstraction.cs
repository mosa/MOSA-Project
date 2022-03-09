using Mosa.FileSystem;
using Mosa.Kernel.Impl;

namespace Mosa.Demo.CoolWorld.x86.Impl
{
	public class KernelAbstraction : BaseKernelAbstraction
	{
		public override GenericFileSystem GetCurrentFileSystem()
		{
			return Boot.FAT[Boot.CurrentDrive];
		}
	}
}
