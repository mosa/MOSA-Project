// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Drivers.Keyboard;

namespace Mosa.DeviceSystem.Devices.Keyboard;

/// <summary>
/// An interface used for interacting with a keyboard. It can retrieve the key codes and states of a keyboard.
/// See <see cref="Keyboard"/> for an implementation that uses an <see cref="IKeyboardDevice"/> and an <see cref="IScanCodeMap"/>
/// underneath.
/// </summary>
public interface IKeyboard
{
	bool ScrollLock { get; set; }

	bool CapsLock { get; set; }

	bool NumLock { get; set; }

	bool Control { get; }

	bool Alt { get; }

	bool Shift { get; }

	bool LeftControl { get; }

	bool RightControl { get; }

	bool LeftAlt { get; }

	bool RightAlt { get; }

	bool LeftShift { get; }

	bool RightShift { get; }

	Key GetKeyPressed(bool blocking = false);
}
