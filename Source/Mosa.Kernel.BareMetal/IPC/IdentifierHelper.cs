// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.IPC;

internal class IdentifierHelper
{
	private static int Sequence = 0;

	public uint GetUniqueIdentifier()
	{
		return (uint)System.Threading.Interlocked.Increment(ref Sequence);
	}
}
