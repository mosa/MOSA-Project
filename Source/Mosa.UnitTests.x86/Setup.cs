// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;

namespace Mosa.UnitTests.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Setup
	{
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

			KernelMemory.SetInitialMemory(Address.GCInitialMemory_UnitTest, 0x01000000);

			Runtime.Internal.Setup();
		}

		private static void ForceTestCollection()
		{
			// force assembly to be loaded
			Mosa.UnitTest.Collection.OptimizationTest.OptimizationTest1();
		}
	}
}
