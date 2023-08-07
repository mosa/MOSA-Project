// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.Kernel.BareMetal;

public static class Kernel
{
	public static ServiceManager ServiceManger { get; set; }

	public static Keyboard Keyboard { get; set; }
}
