// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Implements a key
	/// </summary>
	public class Key
	{
		/// <summary>
		/// Gets or sets the special key.
		/// </summary>
		/// <value>The special key.</value>
		public KeyType KeyType { get; set; }

		/// <summary>
		/// Gets or sets the character.
		/// </summary>
		/// <value>The character.</value>
		public char Character { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the control key is pressed.
		/// </summary>
		/// <value><c>true</c> if control key pressed; otherwise, <c>false</c>.</value>
		public bool Control { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the alt key is pressed.
		/// </summary>
		/// <value><c>true</c> if alt key pressed; otherwise, <c>false</c>.</value>
		public bool Alt { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the shift key is pressed.
		/// </summary>
		/// <value><c>true</c> if shift key pressed; otherwise, <c>false</c>.</value>
		public bool Shift { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Key"/> class.
		/// </summary>
		public Key()
		{
			KeyType = KeyType.NoKey;
			Character = (char)0x00;
			Control = false;
			Alt = false;
			Shift = false;
		}
	}
}
