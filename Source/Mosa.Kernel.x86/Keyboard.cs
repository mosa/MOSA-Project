// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Keyboard
	{
		/// <summary>
		/// Reads the scan code from the device
		/// </summary>
		/// <returns></returns>
		public static byte ReadScanCode()
		{
			return Native.In8(0x60);
		}
	}
}