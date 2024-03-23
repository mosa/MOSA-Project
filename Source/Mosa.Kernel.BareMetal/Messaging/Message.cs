// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.Messaging;

public class Message
{
	public readonly object Data;
	public readonly Thread Thread;
	public readonly uint Sequence;

	public Message(object requestData, Thread thread = null, uint sequence = 0)
	{
		Data = requestData;
		Thread = thread;
		Sequence = sequence;
	}
}
