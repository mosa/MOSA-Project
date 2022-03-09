using Mosa.FileSystem;

namespace Mosa.Kernel.Impl
{
	public static class KernelImpl
	{
		private static BaseKernelAbstraction kernelAbstraction;

		public static void Setup(BaseKernelAbstraction abstraction)
		{
			kernelAbstraction = abstraction;
		}

		public static GenericFileSystem GetCurrentFileSystem()
		{
			return kernelAbstraction.GetCurrentFileSystem();
		}
	}
}
