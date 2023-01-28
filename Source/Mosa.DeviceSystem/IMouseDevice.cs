// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
/// Interface to a mouse device
/// </summary>
public interface IMouseDevice
{
	/// <summary>
	/// The X coordinate.
	/// </summary>
	uint X { get; set; }

	/// <summary>
	/// The Y coordinate.
	/// </summary>
	uint Y { get; set; }

	/// <summary>
	/// The mouse state.
	/// </summary>
	MouseState State { get; }

	/// <summary>
	/// Sets the screen resolution inside the mouse to make it work properly.
	/// Set this before using the mouse driver.
	/// </summary>
	/// <param name="width"></param>
	/// <param name="height"></param>
	void SetScreenResolution(uint width, uint height);
}
