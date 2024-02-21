// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Devices.Keyboard;

namespace Mosa.DeviceSystem.Drivers.Keyboard;

public interface IScanCodeMap
{
	KeyEvent ConvertScanCode(byte scancode);
}
