// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x64;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x64;
using Mosa.UnitTests;

namespace Mosa.TestWorld.x64
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		[Plug("Mosa.Runtime.StartUp::SetInitialMemory")]
		public static void SetInitialMemory()
		{
			KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
		}

		public static void Main()
		{
			Screen.Clear();
			Screen.Color = ScreenColor.Yellow;
			Screen.Write('M');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write('A');
			Screen.Write(' ');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write("!");
			Screen.Write(" ");

			while (true)
			{
				Native.Hlt();
			}
		}

		public static bool IncludeUnitTestAssembly()
		{
			return OptimizationTests.OptimizationTest1();
		}
	}
}
