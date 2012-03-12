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
	/// Interface for keyboard
	/// </summary>
	public interface IKeyboard
	{
		/// <summary>
		/// Gets a value indicating whether [the scroll lock key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the scroll lock key is pressed]; otherwise, <c>false</c>.</value>
		bool ScrollLock { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the cap lock key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the cap lock key is pressed]; otherwise, <c>false</c>.</value>
		bool CapLock { get; set; }

		/// <summary>
		/// Gets a value indicating whether [the num lock key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the num lock key is pressed]; otherwise, <c>false</c>.</value>
		bool NumLock { get; set; }

		/// <summary>
		/// Gets a value indicating whether [any left control key is pressed].
		/// </summary>
		/// <value><c>true</c> if [any control key is pressed]; otherwise, <c>false</c>.</value>
		bool Control { get; }

		/// <summary>
		/// Gets a value indicating whether [any alt key is pressed].
		/// </summary>
		/// <value><c>true</c> if [any alt key is pressed]; otherwise, <c>false</c>.</value>
		bool Alt { get; }

		/// <summary>
		/// Gets a value indicating whether [any shift key is pressed].
		/// </summary>
		/// <value><c>true</c> if [any shift key is pressed]; otherwise, <c>false</c>.</value>
		bool Shift { get; }

		/// <summary>
		/// Gets a value indicating whether [the left control key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the left control key is pressed]; otherwise, <c>false</c>.</value>
		bool LeftControl { get; }

		/// <summary>
		/// Gets a value indicating whether [the right control key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the right control key is pressed]; otherwise, <c>false</c>.</value>
		bool RightControl { get; }

		/// <summary>
		/// Gets a value indicating whether [the left alt key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the left alt key is pressed]; otherwise, <c>false</c>.</value>
		bool LeftAlt { get; }

		/// <summary>
		/// Gets a value indicating whether [the right alt key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the right alt key is pressed]; otherwise, <c>false</c>.</value>
		bool RightAlt { get; }

		/// <summary>
		/// Gets a value indicating whether [the left shift key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the left shift key is pressed]; otherwise, <c>false</c>.</value>
		bool LeftShift { get; }

		/// <summary>
		/// Gets a value indicating whether [the right shift key is pressed].
		/// </summary>
		/// <value><c>true</c> if [the right shift key is pressed]; otherwise, <c>false</c>.</value>
		bool RightShift { get; }

		/// <summary>
		/// Gets the key pressed.
		/// </summary>
		/// <returns></returns>
		Key GetKeyPressed();
	}
}
