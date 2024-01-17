// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public delegate void InterruptHandler(uint irq, uint error);

public static class InterruptManager
{
	public static void Setup()
	{
		Debug.WriteLine("InterreuptManager:Setup()");

		InterruptQueue.Setup();
		Platform.Interrupt.Setup();

		Debug.WriteLine("InterreuptManager:Setup() [Exit]");
	}

	public static void SetHandler(InterruptHandler handler)
	{
		Platform.Interrupt.SetHandler(handler);
	}

	public static void ProcessInterrupt(uint interrupt, uint errorCode)
	{
		InterruptQueue.Enqueue(interrupt);
	}
}
