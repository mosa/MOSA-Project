/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.DeviceDrivers.ScanCodeMap
{
	/// <summary>
	/// Implements the US Keyboard map
	/// </summary>
	public class US : IScanCodeMap
	{
		protected byte state;

		/// <summary>
		/// Initializes a new instance of the <see cref="US"/> class.
		/// </summary>
		public US()
		{
		}

		/// <summary>
		/// Convert can code into a key
		/// </summary>
		/// <param name="scancode">The scancode.</param>
		/// <returns></returns>
		public Key ConvertScanCode(byte scancode)
		{
			Key key = new Key();

			// TODO

			return key;
		}

	}
}
