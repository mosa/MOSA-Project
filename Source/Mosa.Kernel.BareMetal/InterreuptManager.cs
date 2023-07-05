// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public delegate void InterruptHandler(uint irq, uint error);

public static class InterreuptManager
{
	public static void Setup()
	{
		Platform.InterruptHandlerSetup();
	}

	public static void SetHandler(InterruptHandler handler)
	{
		Platform.InterruptHandlerSet(handler);
	}
}
