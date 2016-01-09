// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Implements a key event
	/// </summary>
	public struct KeyEvent
	{
		/// <summary>
		///
		/// </summary>
		public enum KeyPressType : byte
		{
			Make,
			Break
		};

		/// <summary>
		/// Gets or sets the special key.
		/// </summary>
		/// <value>The special key.</value>
		public KeyType KeyType { get; set; }

		/// <summary>
		/// Gets or sets the key press.
		/// </summary>
		/// <value>The key press.</value>
		public KeyPressType KeyPress { get; set; }

		/// <summary>
		/// Gets or sets the character.
		/// </summary>
		/// <value>The character.</value>
		public char Character { get; set; }
	}
}
