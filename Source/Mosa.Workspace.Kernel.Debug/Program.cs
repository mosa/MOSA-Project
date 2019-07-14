// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;

namespace Mosa.Workspace.Kernel
{
	internal static class Program
	{
		private static void Main()
		{
			Emulate.Multiboot.Setup(128 * 1024 * 1024); // 128 MB

			Boot.EntryPoint();

			return;
		}
	}
}
