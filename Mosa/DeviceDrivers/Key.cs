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
			Down,
			Release
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
			LShift,
			RShift,
			Alt,
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
		}

		protected Special special;
		protected Press press;
		protected byte character;

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
		public byte Character { get { return character; } set { character = value; } }
	}


}
