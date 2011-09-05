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
	public interface IFindDevice
	{
		/// <summary>
		/// Determines whether the specified device is match.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns>
		/// 	<c>true</c> if the specified device is match; otherwise, <c>false</c>.
		/// </returns>
		bool IsMatch(IDevice device);
	}
}
