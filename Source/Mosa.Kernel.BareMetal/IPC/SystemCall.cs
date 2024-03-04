// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.IPC;

public static class SystemCall
{
	public static object Send(ServiceIdentification serviceID, object data)
	{
		var messageQueue = QueueRegistry.Find(serviceID);

		if (messageQueue == null)
			return null;

		var message = new Message(data, null);

		messageQueue.Add(message);

		var value = Scheduler.SignalSystemCall();

		return value;
	}
}
