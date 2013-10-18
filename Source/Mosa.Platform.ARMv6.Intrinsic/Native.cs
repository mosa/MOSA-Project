/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Runtime.InteropServices;

namespace Mosa.Platform.ARMv6.Intrinsic
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static class Native
	{
		#region Intrinsic

		[DllImportAttribute(@"Mosa.Platform.ARMv6.Intrinsic.Nop, Mosa.Platform.ARMv6")]
		public extern static void Nop();

		#endregion Intrinsic
	}
}