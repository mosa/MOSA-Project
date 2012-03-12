/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


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
		/// 
		/// </summary>
		protected bool scrollLock;

		/// <summary>
		/// 
		/// </summary>
		protected bool capLock;

		/// <summary>
		/// 
		/// </summary>
		protected bool numLock;

		/// <summary>
		/// 
		/// </summary>
		protected bool leftControl;

		/// <summary>
		/// 
		/// </summary>
		protected bool rightControl;

		/// <summary>
		/// 
		/// </summary>
		protected bool leftAlt;

		/// <summary>
		/// 
		/// </summary>
		protected bool rightAlt;

		/// <summary>
		/// 
		/// </summary>
		protected bool leftShift;

		/// <summary>
		/// 
		/// </summary>
		protected bool rightShift;

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard"/> class.
		/// </summary>
		/// <param name="keyboardDevice">The keyboard device.</param>
		/// <param name="scanCodeMap">The scan code map.</param>
		public Keyboard(IKeyboardDevice keyboardDevice, IScanCodeMap scanCodeMap)
		{
			this.keyboardDevice = keyboardDevice;
			this.scanCodeMap = scanCodeMap;
			scrollLock = false;
			capLock = false;
			numLock = false;

			leftControl = false;
			rightControl = false;
			leftAlt = false;
			rightAlt = false;
			leftShift = false;
			rightShift = false;
		}

		/// <summary>
		/// Gets the key pressed.
		/// </summary>
		/// <returns></returns>
		public Key GetKeyPressed()
		{
			for (; ; )
			{
				byte scanCode = keyboardDevice.GetScanCode();

				KeyEvent keyEvent = scanCodeMap.ConvertScanCode(scanCode);

				if (keyEvent.KeyType == KeyType.RegularKey)
				{
					if (keyEvent.KeyPress == KeyEvent.Press.Make)
					{
						Key key = new Key();
						key.KeyType = KeyType.RegularKey;
						key.Character = keyEvent.Character;
						key.Alt = Alt;
						key.Control = Control;
						key.Shift = Shift;
						return key;
					}
				}

				if (keyEvent.KeyType == KeyType.CapsLock)
					capLock = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.NumLock)
					numLock = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.ScrollLock)
					scrollLock = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.LeftControl)
					leftControl = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.RightControl)
					rightControl = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.LeftAlt)
					leftAlt = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.RightAlt)
					rightAlt = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.LeftShift)
					leftShift = (keyEvent.KeyPress == KeyEvent.Press.Make);

				if (keyEvent.KeyType == KeyType.RightShift)
					rightShift = (keyEvent.KeyPress == KeyEvent.Press.Make);
			}
		}

		/// <summary>
		/// Gets a value indicating whether [the scroll lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the scroll lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool ScrollLock { get { return scrollLock; } set { scrollLock = value; } }

		/// <summary>
		/// Gets a value indicating whether [the cap lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the cap lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool CapLock { get { return capLock; } set { capLock = value; } }

		/// <summary>
		/// Gets a value indicating whether [the num lock key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the num lock key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool NumLock { get { return numLock; } set { numLock = value; } }

		/// <summary>
		/// Gets a value indicating whether [the left control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftControl { get { return leftControl; } set { leftControl = value; } }

		/// <summary>
		/// Gets a value indicating whether [the right control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightControl { get { return rightControl; } set { rightControl = value; } }

		/// <summary>
		/// Gets a value indicating whether [the left alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftAlt { get { return leftAlt; } set { leftAlt = value; } }

		/// <summary>
		/// Gets a value indicating whether [the right alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightAlt { get { return rightAlt; } set { rightAlt = value; } }

		/// <summary>
		/// Gets a value indicating whether [the left shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the left shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool LeftShift { get { return leftShift; } set { leftShift = value; } }

		/// <summary>
		/// Gets a value indicating whether [the right shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [the right shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool RightShift { get { return rightShift; } set { rightShift = value; } }

		/// <summary>
		/// Gets a value indicating whether [any left control key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any control key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Control { get { return rightControl | leftControl; } set { leftControl = value; rightControl = false; } }

		/// <summary>
		/// Gets a value indicating whether [any alt key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any alt key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Alt { get { return rightAlt | leftAlt; } set { leftAlt = value; rightAlt = false; } }

		/// <summary>
		/// Gets a value indicating whether [any shift key is pressed].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any shift key is pressed]; otherwise, <c>false</c>.
		/// </value>
		public bool Shift { get { return rightShift | leftShift; } set { leftShift = value; rightShift = false; } }
	}
}
