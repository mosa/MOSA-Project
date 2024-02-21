// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Devices.Keyboard;

/// <summary>
/// Generally used to describe modifier keys, or just the type of key, like a regular key.
/// </summary>
public enum KeyType : byte
{
	NoKey,
	RegularKey,
	NumLock,
	ScrollLock,
	Home,
	UpArrow,
	PageUp,
	LeftArrow,
	RightArrow,
	End,
	DownArrow,
	PageDown,
	Insert,
	Delete,
	LeftShift,
	RightShift,
	RightAlt,
	LeftAlt,
	CapsLock,
	F1,
	F2,
	F3,
	F4,
	F5,
	F6,
	F7,
	F8,
	F9,
	F10,
	F11,
	F12,
	LeftControl,
	RightControl,
	LeftWindow,
	RightWindow,
	Power,
	Sleep,
	Wake,
	Menu,
	PrintScreen,
	ControlPrintScreen,
	KeyboardError,
	Unknown
}
