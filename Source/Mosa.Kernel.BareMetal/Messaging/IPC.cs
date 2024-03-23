// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.Messaging;

public static class IPC
{
	public static MessageQueue CreateServiceQueue(string serviceName, ServiceIdentification serviceID)
	{
		var messageQueue = new MessageQueue(serviceName, serviceID);

		ServiceQueueRegistry.Register(serviceID, messageQueue);

		return messageQueue;
	}

	public static MessageQueue FindServiceQueue(ServiceIdentification serviceID)
	{
		return ServiceQueueRegistry.Find(serviceID);
	}

	public static MessageQueue FindServiceQueue(string serviceName)
	{
		return ServiceQueueRegistry.Find(serviceName);
	}

	public static object SendData(MessageQueue messageQueue, object data)
	{
		if (messageQueue == null)
			return null;

		Scheduler.SendMessage(messageQueue, data, false);

		var value = Scheduler.SignalSystemCall();

		return value;
	}

	public static object SendDataAsync(MessageQueue messageQueue, object data)
	{
		if (messageQueue == null)
			return null;

		Scheduler.SendMessage(messageQueue, data, true);

		var value = Scheduler.SignalSystemCall();

		return value;
	}

	internal static void SendResponse(Message message, object response)
	{
		// TODO
	}

	internal static Message Listen()
	{
		// TODO

		return null;
	}
}
