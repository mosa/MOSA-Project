// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Graphics;

/// <summary>
/// An interface used for interacting directly with a graphics device. It provides access to the device's frame buffer, along with
/// hardware-accelerated operations like hardware cursors if supported. It can also set the resolution (mode), enable/disable the
/// device, and update specific parts of the underlying buffer.
/// </summary>
public interface IGraphicsDevice
{
	FrameBuffer32 FrameBuffer { get; }

	void SetMode(uint width, uint height);

	void Disable();

	void Enable();

	void Update(uint x, uint y, uint width, uint height);

	void CopyRectangle(uint x, uint y, uint newX, uint newY, uint width, uint height);

	bool SupportsHardwareCursor();

	void DefineCursor(FrameBuffer32 image);

	void SetCursor(bool visible, uint x, uint y);
}
