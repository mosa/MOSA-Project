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
		/// <summary>Gets the width in pixels</summary>
		/// <value>The width.</value>
		public uint Width { get; protected set; }

		/// <summary>Gets the height in pixels</summary>
		/// <value>The height.</value>
		public uint Height { get; protected set; }

		/// <summary>The offset</summary>
		protected uint offset;

		/// <summary>The bytes per pixel</summary>
		protected uint bytesPerPixel;

		/// <summary>The bytes per line</summary>
		protected uint bytesPerLine;

		/// <summary>The memory</summary>
		public ConstrainedPointer Buffer { get; protected set; }

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
			long wi = Math.Clamp(image.Width, 0, Width - x);
			long he = Math.Clamp(image.Height, 0, Height - y);

			if (x < 0 || y < 0 || x >= Width || y >= Height)
				return;

			for (uint h = 0; h < he; h++)
				for (uint w = 0; w < wi; w++)
				{
					uint col = image.RawData.Load32((uint)(wi * h + w));

					if (col != transparentColor)
						SetPixel(col, x + w, y + h);
				}
		}

		/// <summary>Draws an image with alpha or not.</summary>
		/// <param name="image">The image.</param>
		/// <param name="x">X of the top left of the image.</param>
		/// <param name="y">Y of the top left of the image.</param>
		/// <param name="alpha">Draw the image with alpha (from the background color).</param>
		public void DrawImage(Image image, uint x, uint y, bool alpha)
		{
			int wb = image.Width * image.Bpp;
			uint count = (uint)Math.Clamp(wb, 0, (Width - x) * image.Bpp);

			for (int h = 0; h < Math.Clamp(image.Height, 0, Height - y); h++)
			{
				Internal.MemoryCopy(
					Buffer.Address + ((Width * (y + h) + x) * bytesPerPixel),
					image.RawData + (wb * h),
					count);
			}
		}

		/// <summary>Fills a rectangle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="w">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		public void FillRectangle(uint color, uint x, uint y, uint w, uint h)
		{
			w = Math.Clamp(w, 0, Width - x);
			h = Math.Clamp(h, 0, Height - y);

			// TODO: Also clamp X and Y

			if (x >= Width || y >= Height)
				return;

			if (x < 0)
			{
				w += x;
				x = 0;
			}

			if (y < 0)
			{
				h += y;
				y = 0;
			}

			uint wb = w * bytesPerPixel;

			for (int he = 0; he < h; he++)
			{
				Internal.MemoryClear(
					Buffer.Address + ((Width * (y + he) + x) * bytesPerPixel),
					wb, color);
			}
		}

		/* Functions from Cosmos (not all of them are currently there, TODO) */

		/// <summary>Fills a circle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the circle.</param>
		/// <param name="y">Y of the top left of the circle.</param>
		/// <param name="r">Radius of the circle.</param>
		public void FillCircle(uint color, uint x, uint y, uint r)
		{
			if (x < 0 || y < 0 || x >= Width || y >= Height)
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

		public void CopyFrame(IFrameBuffer source)
		{
			Internal.MemoryCopy(Buffer.Address, source.Buffer.Address, Buffer.Size);
		}

		/// <summary>Clears the screen with a specified color.</summary>
		public void ClearScreen(uint color)
		{
			Internal.MemoryClear(Buffer.Address, Buffer.Size, color);
		}
	}
}
