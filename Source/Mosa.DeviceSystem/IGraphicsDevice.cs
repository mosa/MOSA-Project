// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a graphics device
	/// </summary>
	public interface IGraphicsDevice
	{
		/// <summary>The frame buffer.</summary>
		public FrameBuffer32 FrameBuffer { get; }

		/// <summary>Sets the mode.</summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public void SetMode(uint width, uint height);

		/// <summary>Disables the graphics device.</summary>
		public void Disable();

		/// <summary>Enables the graphics device.</summary>
		public void Enable();

		/// <summary>Updates parts of the screen.</summary>
		public void Update(uint x, uint y, uint width, uint height);

		/// <summary>Copies a rectangle on the screen.</summary>
		public void CopyRectangle(uint x, uint y, uint newX, uint newY, uint width, uint height);

		/// <summary>Checks if the device supports hardware-drawn cursors.</summary>
		public bool SupportsHardwareCursor();

		/// <summary>Defines an alpha-based bitmap cursor.</summary>
		public void DefineCursor(FrameBuffer32 image);

		/// <summary>Sets the cursor visibility, X and Y.</summary>
		public void SetCursor(bool visible, uint x, uint y);
	}
}
