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
	/// Implements a key 
	/// </summary>
	public class Key
	{
		/// <summary>
		/// 
		/// </summary>
		protected KeyType keyType;

		/// <summary>
		/// 
		/// </summary>
		protected char character;

		/// <summary>
		/// 
		/// </summary>
		protected bool control;

		/// <summary>
		/// 
		/// </summary>
		protected bool alt;

		/// <summary>
		/// 
		/// </summary>
		protected bool shift;

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		public Key()
		{
			keyType = KeyType.NoKey;
			character = (char)0x00;
			control = false;
			alt = false;
			shift = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		public Key(char key)
		{
			keyType = KeyType.RegularKey;
			character = key;
			control = false;
			alt = false;
			shift = char.IsUpper(key);
		}

		/// <summary>
		/// Gets or sets the special key.
		/// </summary>
		/// <value>The special key.</value>
		public KeyType KeyType { get { return keyType; } set { keyType = value; } }

		/// <summary>
		/// Gets or sets the character.
		/// </summary>
		/// <value>The character.</value>
		public char Character { get { return character; } set { character = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether the control key is pressed.
		/// </summary>
		/// <value><c>true</c> if control key pressed; otherwise, <c>false</c>.</value>
		public bool Control { get { return control; } set { control = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether the alt key is pressed.
		/// </summary>
		/// <value><c>true</c> if alt key pressed; otherwise, <c>false</c>.</value>	
		public bool Alt { get { return alt; } set { alt = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether the shift key is pressed.
		/// </summary>
		/// <value><c>true</c> if shift key pressed; otherwise, <c>false</c>.</value>
		public bool Shift { get { return shift; } set { shift = value; } }
	}


}
