// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.IPC;

public static class SystemCall
{
	public static object Send(object obj, uint receiverID, uint senderID)
	{
		// Find object in memory
		PinObject(obj);

		// Raise the interrupt
		var value = Scheduler.SignalSystemCall(obj, receiverID, senderID);

		UnpinObject(obj);

		return value;
	}

	private static void PinObject(object obj)
	{
		// TODO
	}

	private static void UnpinObject(object obj)
	{
		// TODO
	}
}
