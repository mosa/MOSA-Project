/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.AVR32
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static class Native
	{
		#region Intrinsic

		[DllImportAttribute(@"Mosa.Platform.AVR32.Intrinsic.Nop, Mosa.Platform.AVR32")]
		public extern static void Nop();

		#endregion Intrinsic
	}
}