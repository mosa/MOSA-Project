// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Kernel.BareMetal.IPC;

internal class MessageQueue
{
	public string Name { get; private set; }

	public ServiceIdentification ServiceID { get; private set; }

	private readonly Queue<Message> Queue = new();

	public MessageQueue(string name, ServiceIdentification serviceID)
	{
		Name = name;
		ServiceID = serviceID;
	}

	public void Add(Message message)
	{
		lock (this)
		{
			Queue.Enqueue(message);
		}
	}

	public Message Pop()
	{
		lock (this)
		{
			if (Queue.Count == 0)
				return null;

			return Queue.Dequeue();
		}
	}
}
