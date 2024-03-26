// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Kernel.BareMetal.IPC;

internal static class ServiceQueueRegistry
{
	public static List<MessageQueue> Queues = new();

	public static void Register(ServiceIdentification serviceID, MessageQueue queue)
	{
		lock (Queues)
		{
			Queues.Add(queue);
		}
	}

	public static MessageQueue Find(ServiceIdentification serviceID)
	{
		lock (Queues)
		{
			foreach (var queue in Queues)
			{
				if (queue.ServiceID.ID == serviceID.ID)
					return queue;
			}

			return null;
		}
	}
}
