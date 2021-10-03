// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IFrameBuffer
	{
		/// <summary>Gets the width.</summary>
		/// <value>The width.</value>
		public uint Width { get; }

		/// <summary>Gets the height.</summary>
		/// <value>The height.</value>
		public uint Height { get; }

		/// <summary>Gets the pixel.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public uint GetPixel(uint x, uint y);

		/// <summary>Sets the pixel.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void SetPixel(uint color, uint x, uint y);

		/// <summary>Fills a rectangle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="w">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		public void FillRectangle(uint color, uint x, uint y, uint w, uint h);

		/// <summary>If using double buffering, copies the second buffer to first buffer (screen).</summary>
		public void SwapBuffers();

		/// <summary>Clears the screen with a specified color.</summary>
		public void ClearScreen(uint color);

		//TODO: Add more methods
	}
}
