// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Plug.Korlib.System.Runtime.Intrinsics.X86
{
	internal static class LzcntPlug
	{
		[Plug("System.Runtime.Intrinsics.X86.Lzcnt::LeadingZeroCount")]
		internal static uint LeadingZeroCount(uint value)
		{
			return Native.Lzcnt32(value);
		}
	}
}
