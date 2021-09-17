// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Frame Buffer
	/// </summary>
	/// <seealso cref="Mosa.DeviceSystem.IFrameBuffer" />
	public abstract class FrameBuffer : IFrameBuffer
	{
		/// <summary>
		/// The width
		/// </summary>
		protected uint width;

		/// <summary>
		/// The height
		/// </summary>
		protected uint height;

		/// <summary>
		/// The memory
		/// </summary>
		protected ConstrainedPointer buffer;

		/// <summary>
		/// The offset
		/// </summary>
		protected uint offset;

		/// <summary>
		/// The depth
		/// </summary>
		protected uint depth;

		/// <summary>
		/// Is double buffering
		/// </summary>
		protected bool doubleBuffering;

		/// <summary>
		/// Second buffer for double buffering
		/// </summary>
		protected ConstrainedPointer secondBuffer;

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public uint Width { get { return width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public uint Height { get { return height; } }

		/// <summary>
		/// Checks if the frame buffer is double buffered.
		/// </summary>
		/// <value>The height.</value>
		public bool DoubleBuffering { get { return doubleBuffering; } }

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		protected abstract uint GetOffset(uint x, uint y);

		/// <summary>
		/// Gets the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public abstract uint GetPixel(uint x, uint y);

		/// <summary>
		/// Sets the pixel.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public abstract void SetPixel(uint color, uint x, uint y);

		/// <summary>
		/// Fills a rectangle with color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="w">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		public abstract void FillRectangle(uint color, uint x, uint y, uint w, uint h);

		/// <summary>
		/// If using double buffering, copies the second buffer to the screen.
		/// </summary>
		public void Update()
		{
			if (doubleBuffering)
				Internal.MemoryCopy(buffer.Address, secondBuffer.Address, buffer.Size);
		}

		/// <summary>
		/// Clears the screen with a specified color.
		/// </summary>
		public void Clear(uint color)
		{
			if (doubleBuffering)
				Internal.MemoryClear(secondBuffer.Address, secondBuffer.Size, color);
			else
				Internal.MemoryClear(buffer.Address, buffer.Size, color);
		}
	}
}
