// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.PCI
{
	public class NVMe : BaseDeviceDriver
	{
		public override void Initialize()
		{
			Device.Name = "NVMe";

			HAL.DebugWriteLine("Found NVMe controller!");

			// TODO: 64-bit memory IO
			var mem = Device.Resources.GetMemory(0).Address;

			var cap = mem.Load64(0);
			var vs = mem.Load32(0x08);
			var intms = mem.Load32(0x0C);
			var intmc = mem.Load32(0x10);
			var cc = mem.Load32(0x14);
			var csts = mem.Load32(0x1C);
			var aqa = mem.Load32(0x24);
			var asq = mem.LoadPointer(0x28); // Goes up to 0x2F (47)
			var acq = mem.Load64(0x30);

			HAL.DebugWriteLine("Version: " + vs);
		}
	}
}
