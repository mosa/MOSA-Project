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
		public void SetMode(ushort width, ushort height);

		/// <summary>Disables the graphics device.</summary>
		public void Disable();

		/// <summary>Enables the graphics device.</summary>
		public void Enable();

		/// <summary>Updates the whole screen.</summary>
		public void Update();

		/// <summary>Fills a rectangle on the screen.</summary>
		public void FillRectangle(uint x, uint y, uint width, uint height, uint color);

		public void DefineCursor();

		public void SetCursor(bool visible, uint x, uint y);
	}
}
