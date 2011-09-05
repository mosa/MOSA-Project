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
	/// 
	/// </summary>
	public interface IScanCodeMap
	{
		/// <summary>
		/// Convert can code into a key
		/// </summary>
		/// <param name="scancode">The scancode.</param>
		/// <returns></returns>
		KeyEvent ConvertScanCode(byte scancode);
	}
}
