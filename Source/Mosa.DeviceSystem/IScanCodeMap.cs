// Copyright (c) MOSA Project. Licensed under the New BSD License.

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