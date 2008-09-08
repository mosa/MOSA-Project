/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.DeviceDrivers;

namespace Mosa.EmulatedDevices
{
	/// <summary>
	/// Implements an emulated keyboard
	/// </summary>
	public class Keyboard : Device, IKeyboard
	{
		/// <summary>
		/// Determines whether [scroll lock is on].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [scroll lock is on]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsScrollLockOn() { return false; }

		/// <summary>
		/// Determines whether [cap lock is on].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [cap lock is on]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsCapLockOn() { return Console.CapsLock; }

		/// <summary>
		/// Determines whether [num lock is on].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [num lock is on]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsNumLockOn() { return Console.NumberLock; }

		/// <summary>
		/// Determines whether [the control key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the control key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsControlKeyPressed() { return false; }

		/// <summary>
		/// Determines whether [the alt key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the alt key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsAltKeyPressed() { return false; }

		/// <summary>
		/// Determines whether [the shift key is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [the shift key is pressed]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsShiftKeyPressed() { return false; }

		/// <summary>
		/// Gets the key pressed.
		/// </summary>
		/// <returns></returns>
		public char GetKeyPressed()
		{
			ConsoleKeyInfo keyInfo = Console.ReadKey(true);

			return keyInfo.KeyChar;
		}
	}
}
