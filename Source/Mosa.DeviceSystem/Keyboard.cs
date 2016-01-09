// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Implements a keyboard <see cref="IKeyboard"/>.
	/// </summary>
	public class Keyboard : IKeyboard
	{
		/// <summary>
		///
		/// </summary>
		protected IKeyboardDevice keyboardDevice;

		/// <summary>
		///
		/// </summary>
		protected IScanCodeMap scanCodeMap;

		/// <summary>
		/// Gets a value indicating whether [the scroll lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the scroll lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool ScrollLock { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the cap lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the cap lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool CapLock { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the num lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the num lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool NumLock { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the left control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftControl { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the right control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightControl { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the left alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftAlt { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the right alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightAlt { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the left shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftShift { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the right shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightShift { get; set; }

		/// <summary>
		/// Gets a value indicating whether [any left control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Control { get; set; }

		/// <summary>
		/// Gets a value indicating whether [any alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Alt { get; set; }

		/// <summary>
		/// Gets a value indicating whether [any shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Shift { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard"/> class.
		/// </summary>
		/// <param name="keyboardDevice">The keyboard device.</param>
		/// <param name="scanCodeMap">The scan code map.</param>
		public Keyboard(IKeyboardDevice keyboardDevice, IScanCodeMap scanCodeMap)
		{
			this.keyboardDevice = keyboardDevice;
			this.scanCodeMap = scanCodeMap;
			ScrollLock = false;
			CapLock = false;
			NumLock = false;

			LeftControl = false;
			RightControl = false;
			LeftAlt = false;
			RightAlt = false;
			LeftShift = false;
			RightShift = false;
		}

		/// <summary>
		/// Gets the key pressed.
		/// </summary>
		/// <returns></returns>
		public Key GetKeyPressed()
		{
			byte scanCode = keyboardDevice.GetScanCode();

			if (scanCode == 0)
				return null;

			var keyEvent = scanCodeMap.ConvertScanCode(scanCode);

			if (keyEvent.KeyType == KeyType.RegularKey)
			{
				if (keyEvent.KeyPress == KeyEvent.KeyPressType.Make)
				{
					var key = new Key();
					key.KeyType = keyEvent.KeyType;
					key.Character = keyEvent.Character;
					key.Alt = Alt;
					key.Control = Control;
					key.Shift = Shift;
					return key;
				}
			}

			if (keyEvent.KeyType == KeyType.CapsLock)
				CapLock = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.NumLock)
				NumLock = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.ScrollLock)
				ScrollLock = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.LeftControl)
				LeftControl = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.RightControl)
				RightControl = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.LeftAlt)
				LeftAlt = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.RightAlt)
				RightAlt = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.LeftShift)
				LeftShift = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);
			else if (keyEvent.KeyType == KeyType.RightShift)
				RightShift = (keyEvent.KeyPress == KeyEvent.KeyPressType.Make);

			return null;
		}
	}
}
