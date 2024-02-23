// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Drivers.Keyboard;

namespace Mosa.DeviceSystem.Devices.Keyboard;

/// <summary>
/// An implementation of the <see cref="IKeyboard"/> interface using an <see cref="IKeyboardDevice"/> and an <see cref="IScanCodeMap"/>.
/// </summary>
public class Keyboard : IKeyboard
{
	private readonly IKeyboardDevice keyboardDevice;
	private readonly IScanCodeMap scanCodeMap;

	public bool ScrollLock { get; set; }

	public bool CapsLock { get; set; }

	public bool NumLock { get; set; }

	public bool LeftControl { get; set; }

	public bool RightControl { get; set; }

	public bool LeftAlt { get; set; }

	public bool RightAlt { get; set; }

	public bool LeftShift { get; set; }

	public bool RightShift { get; set; }

	public bool Control { get; set; }

	public bool Alt { get; set; }

	public bool Shift { get; set; }

	public Keyboard(IKeyboardDevice keyboardDevice, IScanCodeMap scanCodeMap)
	{
		this.keyboardDevice = keyboardDevice;
		this.scanCodeMap = scanCodeMap;
		ScrollLock = false;
		CapsLock = false;
		NumLock = false;

		LeftControl = false;
		RightControl = false;
		LeftAlt = false;
		RightAlt = false;
		LeftShift = false;
		RightShift = false;
	}

	public Key GetKeyPressed(bool blocking = false)
	{
		var scanCode = keyboardDevice.GetScanCode(blocking);
		if (scanCode == 0)
			return null;

		var keyEvent = scanCodeMap.ConvertScanCode(scanCode);
		switch (keyEvent.KeyType)
		{
			case KeyType.RegularKey when keyEvent.KeyPress == KeyEvent.KeyPressType.Make:
			{
				var key = new Key
				{
					KeyType = keyEvent.KeyType,
					Character = keyEvent.Character,
					Alt = Alt,
					Control = Control,
					Shift = Shift
				};
				return key;
			}
			case KeyType.CapsLock: CapsLock = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.NumLock: NumLock = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.ScrollLock: ScrollLock = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.LeftControl: LeftControl = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.RightControl: RightControl = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.LeftAlt: LeftAlt = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.RightAlt: RightAlt = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.LeftShift: LeftShift = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
			case KeyType.RightShift: RightShift = keyEvent.KeyPress == KeyEvent.KeyPressType.Make; break;
		}

		return null;
	}
}
