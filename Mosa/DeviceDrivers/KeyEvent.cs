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
	public class KeyEvent
	{
		public enum Press
		{
			Make,
			Break
		};

		protected KeyType keyType;
		protected Press press;
		protected char character;

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		public KeyEvent()
		{
			keyType = KeyType.NoKey;
			press = Press.Make;
			character = (char)0x00;
		}

		/// <summary>
		/// Gets or sets the special key.
		/// </summary>
		/// <value>The special key.</value>
		public KeyType KeyType { get { return keyType; } set { keyType = value; } }

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
