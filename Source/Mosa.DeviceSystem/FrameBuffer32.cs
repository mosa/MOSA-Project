// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;
using System.Drawing;

namespace Mosa.DeviceSystem
{
	/// <summary>Frame Buffer</summary>
	/// <seealso cref="Mosa.DeviceSystem.FrameBuffer32" />
	public sealed class FrameBuffer32
	{
		/// <summary>The bytes per pixel</summary>
		private const uint BytesPerPixel = 4;

		/// <summary>The memory</summary>
		public ConstrainedPointer Buffer { get; }

		/// <summary>Gets the width in pixels</summary>
		/// <value>The width.</value>
		public uint Width { get; }

		/// <summary>Gets the height in pixels</summary>
		/// <value>The height.</value>
		public uint Height { get; }

		/// <summary>The offset</summary>
		public uint Offset { get; }

		/// <summary>The bytes per line</summary>
		public uint BytesPerLine { get; }

		/// <summary>Initializes a new instance of the <see cref="FrameBuffer32"/> class.</summary>
		/// <param name="buffer">The memory.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="bytesPerLine">The bytes per line.</param>
		public FrameBuffer32(ConstrainedPointer buffer, uint width, uint height, uint offset, uint bytesPerLine)
		{
			Buffer = buffer;
			Width = width;
			Height = height;
			Offset = offset;
			BytesPerLine = bytesPerLine;
		}

		/// <summary>Creates a new frame buffer with identical properties.</summary>
		public FrameBuffer32 Clone()
		{
			return new FrameBuffer32(HAL.AllocateMemory(Buffer.Size, 0), Width, Height, Offset, BytesPerLine);
		}

		/// <summary>Gets the offset.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		private uint GetOffset(uint x, uint y)
		{
			return Offset + y * BytesPerLine + x * BytesPerPixel;
		}

		/// <summary>Sets the pixel.</summary>
		/// <param name="color"></param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void SetPixel(uint color, uint x, uint y)
		{
			if (x >= Width || y >= Height)
				return;

			Buffer.Write32(GetOffset(x, y), color);
		}

		/// <summary>Gets the pixel.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public uint GetPixel(uint x, uint y)
		{
			if (x >= Width || y >= Height)
				return 0;

			return Buffer.Read32(GetOffset(x, y));
		}

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
		public void DrawImage(Image image, uint x, uint y, uint transparentColor)
		{
			var wi = Math.Clamp(image.Width, 0, Width - x);
			var he = Math.Clamp(image.Height, 0, Height - y);

			if (x >= Width || y >= Height)
				return;

			for (uint h = 0; h < he; h++)
				for (uint w = 0; w < wi; w++)
				{
					var color = image.GetColor(w, h);

					if (color != transparentColor)
						SetPixel(color, x + w, y + h);
				}
		}

		/// <summary>Draws an image with or without alpha blending.</summary>
		/// <param name="image">The image.</param>
		/// <param name="x">X of the top left of the image.</param>
		/// <param name="y">Y of the top left of the image.</param>
		/// <param name="alpha">Draw the image with or without alpha blending.</param>
		public void DrawImage(Image image, uint x, uint y, bool alpha = false)
		{
			if (alpha)
			{
				// Slow, find faster method (maybe?)

				var wi = Math.Clamp(image.Width, 0, Width - x);
				var he = Math.Clamp(image.Height, 0, Height - y);

				if (x >= Width || y >= Height)
					return;

				for (uint h = 0; h < he; h++)
					for (uint w = 0; w < wi; w++)
					{
						var xx = x + w;
						var yy = y + h;

						SetPixel((uint)AlphaBlend(xx, yy, image.GetColor(w, h)), xx, yy);
					}
			}
			else
			{
				var wb = image.Width * image.BytesPerPixel;
				var count = Math.Clamp(wb, 0, (Width - x) * image.BytesPerPixel);

				for (var h = 0; h < Math.Clamp(image.Height, 0, Height - y); h++)
					Internal.MemoryCopy(
						Buffer.Address + (Width * (y + h) + x) * BytesPerPixel,
						image.Pixels.Address + wb * h,
						count);
			}
		}

		private int AlphaBlend(uint x, uint y, uint color)
		{
			// TODO - replace without using the Color class

			var foreground = Color.FromArgb((int)color);
			var background = Color.FromArgb((int)GetPixel(x, y));

			int alphac = foreground.A;
			var invAlpha = 255 - alphac;

			var newR = (byte)(((foreground.R * alphac + invAlpha * background.R) >> 8) & 0xFF);
			var newG = (byte)(((foreground.G * alphac + invAlpha * background.G) >> 8) & 0xFF);
			var newB = (byte)(((foreground.B * alphac + invAlpha * background.B) >> 8) & 0xFF);

			return Color.ToArgb(newR, newG, newB);
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

			if (x >= Width || y >= Height)
				return;

			var wb = w * BytesPerPixel;

			for (var he = 0; he < h; he++)
				Internal.MemorySet(
					Buffer.Address + (Width * (y + he) + x) * BytesPerPixel,
					color, wb);
		}

		/* Functions from Cosmos (not all of them are currently there, TODO) */

		/// <summary>Fills a circle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the circle.</param>
		/// <param name="y">Y of the top left of the circle.</param>
		/// <param name="r">Radius of the circle.</param>
		public void FillCircle(uint color, uint x, uint y, uint r)
		{
			if (x >= Width || y >= Height)
				return;

			uint x1 = r, y1 = 0, xChange = 1 - (r << 1), yChange = 0, radiusError = 0;

			while (x1 >= y1)
			{
				for (var i = x - x1; i <= x + x1; i++)
				{
					SetPixel(color, i, y + y1);
					SetPixel(color, i, y - y1);
				}

				for (var i = x - y1; i <= x + y1; i++)
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

		public void CopyFrame(FrameBuffer32 source)
		{
			Internal.MemoryCopy(Buffer.Address, source.Buffer.Address, Buffer.Size);
		}

		/// <summary>Clears the screen with a specified color.</summary>
		public void ClearScreen(uint color)
		{
			Internal.MemorySet(Buffer.Address, color, Buffer.Size);
		}
	}
}
