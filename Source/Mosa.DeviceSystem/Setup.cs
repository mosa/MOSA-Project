// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public static class Setup
{
	public static void Initialize(BaseHardwareAbstraction hardware, HAL.HandleInterrupt handleInterrupt)
	{
		// Set device driver system to the hardware HAL
		HAL.Set(hardware);

		// Set the interrupt handler
		HAL.SetInterruptHandler(handleInterrupt);
	}
}
