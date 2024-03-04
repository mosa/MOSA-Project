// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public static class System
{
	public static object SystemCall(object obj)
	{
		// Find object in memory
		PinObject(obj);

		// Raise the interrupt
		return Scheduler.SignalSystemCall(obj);
	}

	public static void PinObject(object obj)
	{
		// TODO
	}
}
