/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers
{
	/// <summary>
	/// Implements a key event
	/// </summary>
	public class Key
	{
		public enum Press
		{
			Make,
			Break
		};

		public enum Special
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
		}

		protected Special special;
		protected Press press;
		protected char character;

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		public Key()
		{
			special = Key.Special.NoKey;
			press = Press.Make;
			character = (char)0x00;
		}

		/// <summary>
		/// Gets or sets the special key.
		/// </summary>
		/// <value>The special key.</value>
		public Special SpecialKey { get { return special; } set { special = value; } }

		/// <summary>
		/// Gets or sets the key press.
		/// </summary>
		/// <value>The key press.</value>
		public Press KeyPress { get { return press; } set { press = value; } }

		/// <summary>
		/// Gets or sets the character.
		/// </summary>
		/// <value>The character.</value>
		public char Character { get { return character; } set { character = value; } }
	}


}
