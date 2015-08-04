// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.Internal.x86
{
	public static class Native
	{
		public static uint Div(ulong a, uint b)
		{
			return (uint)(a / b);
		}
	}
}
