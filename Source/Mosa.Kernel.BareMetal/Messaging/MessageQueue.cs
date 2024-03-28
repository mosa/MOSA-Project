// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Kernel.BareMetal.Messaging;

internal class MessageQueue
{
	public readonly string Name;

	public readonly ServiceIdentification ServiceIdentification;

	public readonly Thread Thread;

	private readonly Queue<Message> Queue = new();

	public MessageQueue(string name, ServiceIdentification serviceIdentification, Thread thread)
	{
		Name = name;
		ServiceIdentification = serviceIdentification;
		Thread = thread;
	}

	public void Add(Message message)
	{
		lock (this)
		{
			Queue.Enqueue(message);
		}
	}

	//public Message Pop()
	//{
	//	lock (this)
	//	{
	//		if (Queue.Count == 0)
	//			return null;

	//		return Queue.Dequeue();
	//	}
	//}
}
