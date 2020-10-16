// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.ARMv8A32
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static class Native
	{
		#region Intrinsic

		[DllImport("Mosa.Platform.ARMv8A32.Intrinsic::Nop")]
		public extern static void Nop();

		#endregion Intrinsic
	}
}
