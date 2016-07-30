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
			KernelMemory.SetInitialMemory(0x02000000, 0x08000000);

			Runtime.Internal.Setup();
		}

		private static void ForceTestCollection()
		{
			// force assembly to be loaded
			Mosa.UnitTest.Collection.OptimizationTest.OptimizationTest1();
		}
	}
}
