// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Framework;

public enum DeviceStatus : byte
{
	Initializing,
	Available,
	Online,
	NotFound,
	Error,
	Offline,
	Disabled
}
