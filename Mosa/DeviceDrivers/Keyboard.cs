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

		protected bool scrollLock;
		protected bool capLock;
		protected bool numLock;

		/// <summary>
		/// Determines whether [scroll lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [scroll lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsScrollLockPressed() { return scrollLock; }

		/// <summary>
		/// Determines whether [cap lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [cap lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsCapLockPressed() { return capLock; }

		/// <summary>
		/// Determines whether [num lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [num lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsNumLockPressed() { return numLock; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard"/> class.
		/// </summary>
		/// <param name="keyboardDevice">The keyboard device.</param>
		public Keyboard(IKeyboardDevice keyboardDevice)
		{
			this.keyboardDevice = keyboardDevice;
			scrollLock = false;
			capLock = false;
			numLock = false;
		}

	}
}
