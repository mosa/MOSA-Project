// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Plug.Korlib.System.Runtime.Intrinsics.X64;

internal static class PopcntPlug
{
	[Plug("System.Runtime.Intrinsics.X64.Popcnt::PopCount")]
	internal static uint PopCount(uint value)
	{
		return Native.Popcnt32(value);
	}
}
