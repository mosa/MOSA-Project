// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Intel;

public class Intel4SeriesChipsetDRAMController : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "Intel4SeriesChipsetDRAMController";
}
