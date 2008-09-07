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
	/// Interface for keyboard
	/// </summary>
	public interface IKeyboard
	{
		/// <summary>
		/// Determines whether [scroll lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [scroll lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		bool IsScrollLockPressed();

		/// <summary>
		/// Determines whether [cap lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [cap lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		bool IsCapLockPressed();

		/// <summary>
		/// Determines whether [num lock is pressed].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [num lock is pressed]; otherwise, <c>false</c>.
		/// </returns>
		bool IsNumLockPressed();


	}
}
