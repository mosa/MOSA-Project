// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.ARMv6
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