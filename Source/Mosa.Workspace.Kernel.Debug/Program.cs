// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Workspace.Kernel.Emulate;

namespace Mosa.Workspace.Kernel
{
	internal static class Program
	{
		private static void Main()
		{
			Multiboot.Setup(128 * 1024 * 1024); // 128 MB
		}
	}
}
