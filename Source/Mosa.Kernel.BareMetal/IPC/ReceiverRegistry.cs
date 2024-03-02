// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.IPC;

internal static class ReceiverRegistry
{
	public static uint RegisterThread(Thread thread)
	{
		var id = IdentifierHelper.GetUniqueIdentifier();

		thread.ReceivedID = id;

		return id;
	}
}
