// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Kernel.BareMetal.IPC;

internal static class QueueRegistry
{
	public static List<MessageQueue> Queues = new List<MessageQueue>();

	//public static Register(ServiceIdentification serviceID, MessageQueue queue)
	//{
	//}

	public static MessageQueue Find(ServiceIdentification serviceID)
	{
		foreach (var queue in Queues)
		{
			if (queue.ServiceID.ID == serviceID.ID)
				return queue;
		}

		return null;
	}
}
