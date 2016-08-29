// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

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
			Runtime.Internal.Setup();

			while (true)
			{
				Native.Hlt();
			}
		}

		private static void ForceTestCollection()
		{
			// required to force assembly to be referenced and loaded
			Mosa.UnitTest.Collection.OptimizationTest.OptimizationTest1();
		}
	}
}
