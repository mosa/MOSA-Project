// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TestWorld.ARMv6
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.ARMv6.Kernel.Setup();
			Mosa.Runtime.ARMv6.Native.Nop();  // force load of the assembly
		}

		public static void Test()
		{
			Mosa.Test.Collection.UInt32Tests.AddU4U4(1, 2);
		}
	}
}
