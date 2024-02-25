// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Keyboard;

public interface IScanCodeMap
{
	KeyEvent ConvertScanCode(byte scancode);
}
