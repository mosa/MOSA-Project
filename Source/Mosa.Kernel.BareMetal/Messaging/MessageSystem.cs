// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Kernel.BareMetal.Messaging;

internal static class MessageSystem
{
	public static Queue<Message> Queue = new();

	public static void DeliverMessage(ServiceIdentification service, bool direction, object requestData)
	{
		var thread = Scheduler.GetCurrentThread();

		// queue the message
		lock (Queue)
		{
			Queue.Enqueue(new Message(service, requestData, direction, thread, 0));
		}
	}

	public static void ProcessMessages()
	{
		//
	}
}
