// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
/// Interface to a graphics device
/// </summary>
public interface IGraphicsDevice
{
	/// <summary>The device name.</summary>
	string Name { get; }

	/// <summary>The frame buffer.</summary>
	FrameBuffer32 FrameBuffer { get; }

	/// <summary>Sets the mode.</summary>
	/// <param name="width">The width.</param>
	/// <param name="height">The height.</param>
	void SetMode(uint width, uint height);

	/// <summary>Disables the graphics device.</summary>
	void Disable();

	/// <summary>Enables the graphics device.</summary>
	void Enable();

	/// <summary>Updates parts of the screen.</summary>
	void Update(uint x, uint y, uint width, uint height);

	/// <summary>Copies a rectangle on the screen.</summary>
	void CopyRectangle(uint x, uint y, uint newX, uint newY, uint width, uint height);

	/// <summary>Checks if the device supports hardware-drawn cursors.</summary>
	bool SupportsHardwareCursor();

	/// <summary>Defines an alpha-based bitmap cursor.</summary>
	void DefineCursor(FrameBuffer32 image);

	/// <summary>Sets the cursor visibility, X and Y.</summary>
	void SetCursor(bool visible, uint x, uint y);
}
