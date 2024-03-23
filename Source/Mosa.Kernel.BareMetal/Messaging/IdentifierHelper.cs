// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.Messaging;

internal static class IdentifierHelper
{
	private static int Sequence = 0;

	public static uint GetUniqueIdentifier()
	{
		return (uint)System.Threading.Interlocked.Increment(ref Sequence);
	}
}
