// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// Intel
	/// </summary>
	public class Intel4SeriesChipsetPCIExpressRootPort : BaseDeviceDriver
	{
		public override void Initialize()
		{
			Device.Name = "Intel4SeriesChipsetPCIExpressRootPort";
		}
	}
}
