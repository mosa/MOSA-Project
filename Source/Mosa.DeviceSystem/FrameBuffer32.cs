// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Drawing;
using Mosa.Runtime;

namespace Mosa.DeviceSystem;

/// <summary>Frame Buffer</summary>
/// <seealso cref="Mosa.DeviceSystem.FrameBuffer32" />
public sealed class FrameBuffer32
{
	/// <summary>The bytes per pixel</summary>
	private const uint BytesPerPixel = 4;

	/// <summary>The memory</summary>
	private ConstrainedPointer Buffer { get; }

	/// <summary>Gets the width in pixels</summary>
	/// <value>The width.</value>
	public uint Width { get; }

	/// <summary>Gets the height in pixels</summary>
	/// <value>The height.</value>
	public uint Height { get; }

	/// <summary>Gets the offset.</summary>
	private readonly Func<uint, uint, uint> GetOffset;

	/// <summary>Initializes a new instance of the <see cref="FrameBuffer32"/> class.</summary>
	/// <param name="buffer">The memory.</param>
	/// <param name="width">The width.</param>
	/// <param name="height">The height.</param>
	public FrameBuffer32(ConstrainedPointer buffer, uint width, uint height, Func<uint, uint, uint> getOffset)
	{
		Buffer = buffer;
		Width = width;
		Height = height;
		GetOffset = getOffset;
	}

	/// <summary>Creates a new frame buffer with identical properties.</summary>
	public FrameBuffer32 Clone()
	{
		return new FrameBuffer32(HAL.AllocateMemory(Buffer.Size, 0), Width, Height, GetOffset);
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

	/// <summary>Draws a virtual buffer with a transparent color.</summary>
	/// <param name="buffer">The virtual buffer.</param>
	/// <param name="x">X of the top left of the image.</param>
	/// <param name="y">Y of the top left of the image.</param>
	/// <param name="transparentColor">Transparent color, to not draw.</param>
	public void DrawBuffer(FrameBuffer32 buffer, uint x, uint y, uint transparentColor)
	{
		var wi = Math.Clamp(buffer.Width, 0, Width - x);
		var he = Math.Clamp(buffer.Height, 0, Height - y);

		if (x >= Width || y >= Height)
			return;

		for (var h = 0u; h < he; h++)
			for (var w = 0u; w < wi; w++)
			{
				var color = buffer.GetPixel(w, h);

				if (color != transparentColor)
					SetPixel(color, x + w, y + h);
			}
	}

	/// <summary>Draws a virtual buffer with or without alpha blending.</summary>
	/// <param name="buffer">The virtual buffer.</param>
	/// <param name="x">X of the top left of the image.</param>
	/// <param name="y">Y of the top left of the image.</param>
	/// <param name="alpha">Draw the image with or without alpha blending.</param>
	public void DrawBuffer(FrameBuffer32 buffer, uint x, uint y, bool alpha)
	{
		if (alpha)
		{
			// Slow, find faster method (maybe?)

			var wi = Math.Clamp(buffer.Width, 0, Width - x);
			var he = Math.Clamp(buffer.Height, 0, Height - y);

			if (x >= Width || y >= Height)
				return;

			for (var h = 0u; h < he; h++)
				for (var w = 0u; w < wi; w++)
				{
					var xx = x + w;
					var yy = y + h;

					SetPixel(AlphaBlend(xx, yy, buffer.GetPixel(w, h)), xx, yy);
				}
		}
		else
		{
			var wb = buffer.Width * 4;
			var count = Math.Clamp(wb, 0, (Width - x) * 4);

			for (var h = 0; h < Math.Clamp(buffer.Height, 0, Height - y); h++)
				Internal.MemoryCopy(
					Buffer.Address + (Width * (y + h) + x) * BytesPerPixel,
					buffer.Buffer.Address + wb * h,
					count);
		}
	}

	private uint AlphaBlend(uint x, uint y, uint color)
	{
		// TODO - replace without using the Color class

		var foreground = Color.FromArgb((int)color);
		var background = Color.FromArgb((int)GetPixel(x, y));

		int alphac = foreground.A;
		var invAlpha = 255 - alphac;

		var newR = (byte)(((foreground.R * alphac + invAlpha * background.R) >> 8) & 0xFF);
		var newG = (byte)(((foreground.G * alphac + invAlpha * background.G) >> 8) & 0xFF);
		var newB = (byte)(((foreground.B * alphac + invAlpha * background.B) >> 8) & 0xFF);

		return (uint)Color.ToArgb(newR, newG, newB);
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

			if ((radiusError << 1) + xChange > 0)
			{
				x1--;
				radiusError += xChange;
				xChange += 2;
			}
		}
	}

	public void DrawLine(uint color, uint x1, uint y1, uint x2, uint y2)
	{
		var dx = x2 - x1; /* The horizontal distance of the line */
		var dy = y2 - y1; /* The vertical distance of the line */

		if (dy == 0) /* The line is horizontal */
		{
			DrawHorizontalLine(color, dx, x1, y1);
			return;
		}

		if (dx == 0) /* The line is vertical */
		{
			DrawVerticalLine(color, dy, x1, y1);
			return;
		}

		/* The line is neither horizontal nor vertical, so it's diagonal! */
		DrawDiagonalLine(color, dx, dy, x1, y1);
	}

	private void DrawHorizontalLine(uint color, uint dx, uint x, uint y)
	{
		for (var i = 0u; i < dx; i++)
			SetPixel(x + i, y, color);
	}

	private void DrawVerticalLine(uint color, uint dy, uint x, uint y)
	{
		for (var i = 0u; i < dy; i++)
			SetPixel(color, x, y + i);
	}

	private void DrawDiagonalLine(uint color, uint dx, uint dy, uint x1, uint y1)
	{
		var sdx = (uint)Math.Sign(dx);
		var sdy = (uint)Math.Sign(dy);
		var x = dy >> 1;
		var y = dx >> 1;
		var px = x1;
		var py = y1;

		if (dx >= dy) /* The line is more horizontal than vertical */
		{
			for (var i = 0; i < dx; i++)
			{
				y += dy;
				if (y >= dx)
				{
					y -= dx;
					py += sdy;
				}
				px += sdx;
				SetPixel(color, px, py);
			}
		}
		else /* The line is more vertical than horizontal */
		{
			for (var i = 0; i < dy; i++)
			{
				x += dx;
				if (x >= dy)
				{
					x -= dy;
					px += sdx;
				}
				py += sdy;
				SetPixel(color, px, py);
			}
		}
	}

	/// <summary>Copies a source framebuffer to the current one.</summary>
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
