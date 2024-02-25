// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Keyboard;

/// <summary>
/// The key code class notably used by <see cref="IKeyboard"/>.
/// </summary>
public class Key
{
	public KeyType KeyType { get; set; } = KeyType.NoKey;

	public char Character { get; set; } = (char)0x00;

	public bool Control { get; set; }

	public bool Alt { get; set; }

	public bool Shift { get; set; }
}
