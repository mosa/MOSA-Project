// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Devices.Keyboard;

namespace Mosa.Kernel.BareMetal;

public static class Kernel
{
	public static ServiceManager ServiceManager { get; set; }

	public static Keyboard Keyboard { get; set; } // temporary hack
}
