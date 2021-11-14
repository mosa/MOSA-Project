// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a graphics device
	/// </summary>
	public interface IGraphicsDevice
	{
		/// <summary>The frame buffer.</summary>
		public IFrameBuffer FrameBuffer { get; }

		/// <summary>Sets the mode.</summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="bitsPerPixel">The amount of bits per pixel.</param>
		public void SetMode(ushort width, ushort height, byte bitsPerPixel = 32);

		/// <summary>Disables the graphics device.</summary>
		public void Disable();

		/// <summary>Enables the graphics device.</summary>
		public void Enable();

		/// <summary>Updates the whole screen.</summary>
		public void Update();
	}
}