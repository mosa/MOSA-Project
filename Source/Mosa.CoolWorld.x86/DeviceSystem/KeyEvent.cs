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
	/// Implements a key event
	/// </summary>
	public class KeyEvent
	{
		/// <summary>
		///
		/// </summary>
		public enum Press
		{
			/// <summary>
			///
			/// </summary>
			Make,

			/// <summary>
			///
			/// </summary>
			Break
		};

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		public KeyEvent()
		{
			KeyType = KeyType.NoKey;
			KeyPress = Press.Make;
			Character = (char)0x00;
		}

		/// <summary>
		/// Gets or sets the special key.
		/// </summary>
		/// <value>The special key.</value>
		public KeyType KeyType;

		/// <summary>
		/// Gets or sets the key press.
		/// </summary>
		/// <value>The key press.</value>
		public Press KeyPress;

		/// <summary>
		/// Gets or sets the character.
		/// </summary>
		/// <value>The character.</value>
		public char Character;
	}
}