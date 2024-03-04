// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Mouse;

/// <summary>
/// An interface used for interacting directly with a mouse device. It provides access to the device's raw X and Y coordinates,
/// as well as its current state. It is important to provide the screen resolution to the mouse using the SetScreenResolution()
/// function first before using the driver.
/// </summary>
public interface IMouseDevice
{
	uint X { get; set; }

	uint Y { get; set; }

	MouseState State { get; }

	/// <summary>
	/// Provides the screen resolution to the mouse device. Set this before using the mouse driver.
	/// </summary>
	/// <param name="width">The screen width.</param>
	/// <param name="height">The screen height.</param>
	void SetScreenResolution(uint width, uint height);
}
