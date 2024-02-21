// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Devices.Keyboard;

/// <summary>
/// Describes a press/release key code event.
/// </summary>
public struct KeyEvent
{
	public enum KeyPressType : byte
	{
		Make,
		Break
	}

	public KeyType KeyType { get; set; }

	public KeyPressType KeyPress { get; set; }

	public char Character { get; set; }
}
