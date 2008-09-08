/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{

	/// <summary>
	/// Implements a keyboard <see cref="IKeyboard"/>.
	/// </summary>
	public class Keyboard : IKeyboard
	{
		protected IKeyboardDevice keyboardDevice;
		protected IScanCodeMap scanCodeMap;

		protected bool scrollLock;
		protected bool capLock;
		protected bool numLock;

		protected bool controlLeftKeyPressed;
		protected bool controlRightKeyPressed;
		protected bool altLeftKeyPressed;
		protected bool altRightKeyPressed;
		protected bool shiftLeftKeyPressed;
		protected bool shiftRightKeyPressed;

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard"/> class.
		/// </summary>
		/// <param name="keyboardDevice">The keyboard device.</param>
		public Keyboard(IKeyboardDevice keyboardDevice, IScanCodeMap scanCodeMap)
		{
			this.keyboardDevice = keyboardDevice;
			this.scanCodeMap = scanCodeMap;
			scrollLock = false;
			capLock = false;
			numLock = false;

			controlLeftKeyPressed = false;
			controlRightKeyPressed = false;
			altLeftKeyPressed = false;
			altRightKeyPressed = false;
			shiftLeftKeyPressed = false;
			shiftRightKeyPressed = false;
		}

		/// <summary>
		/// Gets the key pressed.
		/// </summary>
		/// <returns></returns>
		public char GetKeyPressed()
		{
			for (; ; ) {
				byte scanCode = keyboardDevice.GetScanCode();

				Key key = scanCodeMap.ConvertScanCode(scanCode);

				if (key.SpecialKey == Key.Special.RegularKey)
					return key.Character;

				if (key.SpecialKey == Key.Special.CapsLock)
					capLock = (key.KeyPress == Key.Press.Make);

				if (key.SpecialKey == Key.Special.NumLock)
					numLock = (key.KeyPress == Key.Press.Make);

				if (key.SpecialKey == Key.Special.ScrollLock)
					scrollLock = (key.KeyPress == Key.Press.Make);

				if (key.SpecialKey == Key.Special.LeftControl)
					controlLeftKeyPressed = (key.KeyPress == Key.Press.Make);

				if (key.SpecialKey == Key.Special.RightControl)
					controlRightKeyPressed = (key.KeyPress == Key.Press.Make);
				
				if (key.SpecialKey == Key.Special.LeftAlt)
					altLeftKeyPressed = (key.KeyPress == Key.Press.Make);
				
				if (key.SpecialKey == Key.Special.RightAlt)
					altRightKeyPressed = (key.KeyPress == Key.Press.Make);
				
				if (key.SpecialKey == Key.Special.LeftShift)
					shiftLeftKeyPressed = (key.KeyPress == Key.Press.Make);

				if (key.SpecialKey == Key.Special.RightShift)
					shiftRightKeyPressed = (key.KeyPress == Key.Press.Make);
			}
		}

		/// <summary>
		/// Determines whether [scroll lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [scroll lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsScrollLockOn() { return scrollLock; }

		/// <summary>
		/// Determines whether [cap lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [cap lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsCapLockOn() { return capLock; }

		/// <summary>
		/// Determines whether [num lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [num lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsNumLockOn() { return numLock; }

		/// <summary>
		/// Determines whether [the control key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the control key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsControlKeyPressed() { return (controlLeftKeyPressed || controlRightKeyPressed); }

		/// <summary>
		/// Determines whether [the alt key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the alt key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsAltKeyPressed() { return (altLeftKeyPressed || altRightKeyPressed); }

		/// <summary>
		/// Determines whether [the shift key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the shift key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsShiftKeyPressed() { return (shiftLeftKeyPressed || shiftRightKeyPressed); }
	}
}
