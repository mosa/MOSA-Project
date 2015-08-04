// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a keyboard device
	/// </summary>
	public interface IKeyboardDevice
	{
		/// <summary>
		/// Gets the scan code from the keyboard device
		/// </summary>
		/// <returns></returns>
		byte GetScanCode();
	}
}
