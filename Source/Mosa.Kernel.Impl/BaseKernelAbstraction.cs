using Mosa.FileSystem;

namespace Mosa.Kernel.Impl
{
	public abstract class BaseKernelAbstraction
	{
		public abstract GenericFileSystem GetCurrentFileSystem();
	}
}
