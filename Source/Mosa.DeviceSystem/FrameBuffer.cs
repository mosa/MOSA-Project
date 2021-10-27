// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;
using System.Drawing;

namespace Mosa.DeviceSystem
{
	/// <summary>Frame Buffer</summary>
	/// <seealso cref="Mosa.DeviceSystem.IFrameBuffer" />
	public abstract class FrameBuffer : IFrameBuffer
	{
		/// <summary>The width</summary>
		protected uint width;

		/// <summary>Gets the width in pixels</summary>
		/// <value>The width.</value>
		public uint Width { get { return width; } }

		/// <summary>The height</summary>
		protected uint height;

		/// <summary>Gets the height in pixels</summary>
		/// <value>The height.</value>
		public uint Height { get { return height; } }

		/// <summary>The offset</summary>
		protected uint offset;

		/// <summary>The bytes per pixel</summary>
		protected uint bytesPerPixel;

		/// <summary>The bytes per line</summary>
		protected uint bytesPerLine;

		/// <summary>The memory</summary>
		protected ConstrainedPointer firstBuffer;

		/// <summary>Second buffer for double buffering</summary>
		protected ConstrainedPointer secondBuffer;

		/// <summary>Is double buffering</summary>
		protected bool doubleBuffering;

		/// <summary>Checks if the frame buffer is double buffered.</summary>
		/// <value>The height.</value>
		public bool DoubleBuffering { get { return doubleBuffering; } }

		/// <summary>Gets the offset.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		protected abstract uint GetOffset(uint x, uint y);

		/// <summary>Sets the pixel.</summary>
		/// <param name="color"></param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public abstract void SetPixel(uint color, uint x, uint y);

		/// <summary>Gets the pixel.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public abstract uint GetPixel(uint x, uint y);

		/// <summary>Fills a rectangle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="w">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		public abstract void FillRectangle(uint color, uint x, uint y, uint w, uint h);

		/// <summary>Draws a rectangle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="wi">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		/// <param name="we">Weight of the rectangle.</param>
		public void DrawRectangle(uint color, uint x, uint y, uint wi, uint h, uint we)
		{
			FillRectangle(color, x, y, wi, we);

			FillRectangle(color, x, y, we, h);
			FillRectangle(color, x + (wi - we), y, we, h);

			FillRectangle(color, x, y + (h - we), wi, we);
		}

		/// <summary>Fills a rectangle with rounded corners and color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="w">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		/// <param name="r">Radius of the corners.</param>
		public void DrawFilledRoundedRectangle(uint color, uint x, uint y, uint w, uint h, uint r)
		{
			FillRectangle(color, x + r, y, w - r * 2, h);
			FillRectangle(color, x, y + r, w, h - r * 2);

			FillCircle(color, x + r, y + r, r);
			FillCircle(color, x + w - r - 1, y + r, r);

			FillCircle(color, x + r, y + h - r - 1, r);
			FillCircle(color, x + w - r - 1, y + h - r - 1, r);
		}

		/// <summary>Draws an image with a transparent color.</summary>
		/// <param name="image">The image.</param>
		/// <param name="x">X of the top left of the image.</param>
		/// <param name="y">Y of the top left of the image.</param>
		/// <param name="transparentColor">Transparent color, to not draw.</param>
		public void DrawImage(Image image, uint x, uint y, int transparentColor)
		{
			long wi = Math.Clamp(image.Width, 0, width - x);
			long he = Math.Clamp(image.Height, 0, height - y);

			if (x < 0 || y < 0 || x >= width || y >= height)
				return;

			for (uint h = 0; h < he; h++)
				for (uint w = 0; w < wi; w++)
					if (image.RawData.Load32((uint)(wi * h + w)) != transparentColor)
						SetPixel(image.RawData.Load32((uint)(wi * h + w)), x + w, y + h);
		}

		/// <summary>Draws an image with alpha or not.</summary>
		/// <param name="image">The image.</param>
		/// <param name="x">X of the top left of the image.</param>
		/// <param name="y">Y of the top left of the image.</param>
		/// <param name="alpha">Draw the image with alpha (from the background color).</param>
		public void DrawImage(Image image, uint x, uint y, bool alpha)
		{
			long wi = Math.Clamp(image.Width, 0, width - x);
			long he = Math.Clamp(image.Height, 0, height - y);

			if (x < 0 || y < 0 || x >= width || y >= height)
				return;

			for (uint h = 0; h < he; h++)
				for (uint w = 0; w < wi; w++)
					if (alpha)
					{
						Color foreground = Color.FromArgb((int)image.RawData.Load32((uint)(wi * h + w)));
						Color background = Color.FromArgb((int)GetPixel(x + w, y + h));

						int alphaColor = foreground.A;
						int inv_alpha = 255 - alphaColor;

						byte newR = (byte)(((foreground.R * alphaColor + inv_alpha * background.R) >> 8) & 0xFF);
						byte newG = (byte)(((foreground.G * alphaColor + inv_alpha * background.G) >> 8) & 0xFF);
						byte newB = (byte)(((foreground.B * alphaColor + inv_alpha * background.B) >> 8) & 0xFF);

						SetPixel((uint)Color.ToArgb(newR, newG, newB), x + w, y + h);
					}
					else SetPixel((uint)image.RawData.Load32(((uint)(wi * h + w)) * 4), x + w, y + h);
		}

		/* Functions from Cosmos (not all of them are currently there, TODO) */

		/// <summary>Fills a circle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the circle.</param>
		/// <param name="y">Y of the top left of the circle.</param>
		/// <param name="r">Radius of the circle.</param>
		public void FillCircle(uint color, uint x, uint y, uint r)
		{
			if (x < 0 || y < 0 || x >= width || y >= height)
				return;

			uint x1 = r, y1 = 0, xChange = 1 - (r << 1), yChange = 0, radiusError = 0;

			while (x1 >= y1)
			{
				for (uint i = x - x1; i <= x + x1; i++)
				{
					SetPixel(color, i, y + y1);
					SetPixel(color, i, y - y1);
				}

				for (uint i = x - y1; i <= x + y1; i++)
				{
					SetPixel(color, i, y + x1);
					SetPixel(color, i, y - x1);
				}

				y1++;
				radiusError += yChange;
				yChange += 2;

				if (((radiusError << 1) + xChange) > 0)
				{
					x1--;
					radiusError += xChange;
					xChange += 2;
				}
			}
		}

		/* End functions from Cosmos */

		/// <summary>If using double buffering, copies the second buffer to the screen.</summary>
		public void SwapBuffers()
		{
			if (doubleBuffering)
				Internal.MemoryCopy(firstBuffer.Address, secondBuffer.Address, firstBuffer.Size);
		}

		/// <summary>Clears the screen with a specified color.</summary>
		public void ClearScreen(uint color)
		{
			if (doubleBuffering)
				Internal.MemoryClear(secondBuffer.Address, secondBuffer.Size, color);
			else
				Internal.MemoryClear(firstBuffer.Address, firstBuffer.Size, color);
		}
	}
}
