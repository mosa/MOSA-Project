// Copyright (c) MOSA Project. Licensed under the New BSD License.

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