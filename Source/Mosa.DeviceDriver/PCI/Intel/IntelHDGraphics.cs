using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.PCI.Intel
{
	//https://wiki.osdev.org/User:Greasemonkey/Intel_GenX
	public class IntelHDGraphics : BaseDeviceDriver
	{
		// UNTESTED
		public override unsafe void Initialize()
		{
			Device.Name = "IntelHDGraphics";

			// Skylake graphics are Gen9
			HAL.DebugWriteLine("Found Intel HD Graphics (520)!");

			var bar0 = Device.Resources.GetMemory(0);

			var registers = bar0.Read64(0x10);
			var memory = bar0.Read64(0x18);

			// TODO
		}
	}
}
