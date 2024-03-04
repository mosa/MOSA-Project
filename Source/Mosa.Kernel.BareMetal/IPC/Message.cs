// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.IPC;

internal class Message
{
	public readonly object Data;
	public readonly uint Sequence;
	public readonly Thread Thread;

	public Message(object requestData, Thread thread = null)
	{
		Data = requestData;

		if (thread != null)
		{
			Thread = thread;
			Sequence = ++thread.Sequence;
		}
		else
		{
			Sequence = 0;
		}
	}
}
