// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace Mosa.UnitTests.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		[Method("Mosa.Runtime.StartUp.SetInitialMemory")]
		public static void SetInitialMemory()
		{
			KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
		}

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
			Screen.Color = 0x0E;
			Screen.Write('M');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write('A');
			Screen.Write(' ');

			Runtime.Internal.Setup();

			while (true)
			{
			}
		}

		private static void ForceTestCollection()
		{
			// required to force assembly to be referenced and loaded
			Mosa.UnitTest.Collection.OptimizationTest.OptimizationTest1();
		}
	}
}
