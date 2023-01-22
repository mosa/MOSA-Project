// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Plug.Korlib.x86.System.Runtime.Intrinsics.X86
{
	internal static class Bmi1Plug
	{
		[Plug("System.Runtime.Intrinsics.X86.Bmi1::ResetLowestSetBit")]
		internal static uint ResetLowestSetBit(uint value)
		{
			return Native.Blsr32(value);
		}

		[Plug("System.Runtime.Intrinsics.X86.Bmi1::TrailingZeroCount")]
		internal static uint TrailingZeroCount(uint value)
		{
			return Native.Tzcnt32(value);
		}
	}
}
