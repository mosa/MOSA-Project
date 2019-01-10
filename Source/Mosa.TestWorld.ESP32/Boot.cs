// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;

namespace Mosa.TestWorld.ESP32
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.ESP32.Kernel.Setup();
			Mosa.Runtime.ESP32.Native.Nop();  // force load of the assembly
		}

		public static void Test()
		{
			UInt32Tests.AddU4U4(1, 2);
		}
	}
}
