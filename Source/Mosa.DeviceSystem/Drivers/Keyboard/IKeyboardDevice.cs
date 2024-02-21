// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Drivers.Keyboard;

/// <summary>
/// An interface used for interacting directly with a keyboard device. It provides access to the device's raw scan codes, in a
/// blocking or non-blocking way. Note that the device must obviously support retrieving raw scan codes (e.g. a PS/2 keyboard)
/// </summary>
public interface IKeyboardDevice
{
	byte GetScanCode(bool blocking = false);
}
