// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Intel;

public class IntelEthernetController82540EM : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "IntelEthernetController82540EM";
}
