// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.ARM32;

/// <summary>
/// Provides stub methods for selected x86 native assembly instructions.
/// </summary>
public static class Native
{
	#region Intrinsic

	[DllImport("Mosa.Platform.ARM32.Intrinsic::Nop")]
	public static extern void Nop();

	#endregion Intrinsic
}
