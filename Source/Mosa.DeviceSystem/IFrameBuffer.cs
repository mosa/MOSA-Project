// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IFrameBuffer
	{
		/// <summary>Gets the underlying frame buffer.</summary>
		/// <value>The buffer.</value>
		public ConstrainedPointer Buffer { get; }

		/// <summary>Gets the width.</summary>
		/// <value>The width.</value>
		public uint Width { get; }

		/// <summary>Gets the height.</summary>
		/// <value>The height.</value>
		public uint Height { get; }

		/// <summary>Creates a new frame buffer with identical properties.</summary>
		public IFrameBuffer Clone();

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

		/// <summary>Draws a rectangle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="wi">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		/// <param name="we">Weight of the rectangle.</param>
		public void DrawRectangle(uint color, uint x, uint y, uint wi, uint h, uint we);

		/// <summary>Fills a rectangle with rounded corners and color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="w">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		/// <param name="r">Radius of the corners.</param>
		public void DrawFilledRoundedRectangle(uint color, uint x, uint y, uint w, uint h, uint r);

		/// <summary>Draws an image with a transparent color.</summary>
		/// <param name="image">The image.</param>
		/// <param name="x">X of the top left of the image.</param>
		/// <param name="y">Y of the top left of the image.</param>
		/// <param name="transparentColor">Transparent color, to not draw.</param>
		public void DrawImage(Image image, uint x, uint y, int transparentColor);

		/// <summary>Draws an image with alpha or not.</summary>
		/// <param name="image">The image.</param>
		/// <param name="x">X of the top left of the image.</param>
		/// <param name="y">Y of the top left of the image.</param>
		public void DrawImage(Image image, uint x, uint y, bool alpha = false);

		/// <summary>Fills a circle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the circle.</param>
		/// <param name="y">Y of the top left of the circle.</param>
		/// <param name="r">Radius of the circle.</param>
		public void FillCircle(uint color, uint x, uint y, uint r);

		/// <summary>If using double buffering, copies the second buffer to first buffer (screen).</summary>
		/// <param name="source">The FrameBuffer.</param>
		public void CopyFrame(IFrameBuffer source);

		/// <summary>Clears the screen with a specified color.</summary>
		public void ClearScreen(uint color);
	}
}
