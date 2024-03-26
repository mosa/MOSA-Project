// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.Messaging;

internal struct Message
{
	public readonly ServiceIdentification ServiceIdentification;
	public readonly object Data;
	public readonly bool Direction;
	public readonly uint Sequence;
	public readonly Thread Thread;

	public Message(ServiceIdentification serviceIdentification, object requestData, bool direction, Thread thread, uint sequence)
	{
		ServiceIdentification = serviceIdentification;
		Data = requestData;
		Direction = direction;
		Thread = thread;
		Sequence = sequence;
	}
}
