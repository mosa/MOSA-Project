/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

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
			Mosa.Platform.ARMv6.Intrinsic.Native.Nop();	// force load of the assembly
		}
	}
}