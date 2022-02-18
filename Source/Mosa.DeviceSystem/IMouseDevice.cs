// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a mouse device
	/// </summary>
	public interface IMouseDevice
	{
		int X { get; set; }

		int Y { get; set; }

		/// <summary>
		/// Get the current state of the mouse
		///
		/// 0 = Left
		/// 1 = Right
		/// 2 = Scroll wheel
		/// 255 = None
		/// </summary>
		/// <returns></returns>
		int GetMouseState();

		/// <summary>
		/// Sets the screen resolution inside the mouse to make it work properly.
		/// Set this before using the mouse driver.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		void SetScreenResolution(int width, int height);
	}
}
