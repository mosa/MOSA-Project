// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Intel;

public class Intel4SeriesChipsetIntegratedGraphicsController2E10 : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "Intel4SeriesChipsetIntegratedGraphicsController2E10";
}
