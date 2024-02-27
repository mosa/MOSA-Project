﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Intel;

/// <summary>
/// Intel
/// </summary>
public class Intel4SeriesChipsetIntegratedGraphicsController : BaseDeviceDriver
{
	public override void Initialize()
	{
		Device.Name = "Intel4SeriesChipsetIntegratedGraphicsController";
	}
}
