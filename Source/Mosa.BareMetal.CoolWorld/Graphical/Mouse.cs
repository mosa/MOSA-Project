﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Mosa.DeviceSystem.Graphics;
using Mosa.DeviceSystem.Mouse;

namespace Mosa.BareMetal.CoolWorld.Graphical;

public static class Mouse
{
	public static uint X => Utils.Mouse.X;

	public static uint Y => Utils.Mouse.Y;

	public static MouseState State => Utils.Mouse.State;

	public static Color Color { get; set; }

	public static bool HardwareCursor { get; private set; }

	private static List<uint[]> mouseCols;

	public static void Initialize()
	{
		HardwareCursor = Display.Driver.SupportsHardwareCursor();

		if (HardwareCursor)
		{
			var data = File.ReadAllBytes("cur.bmp");
			var image = Bitmap.CreateImage(data);
			Display.Driver.DefineCursor(image);
		}
		else
		{
			mouseCols = new List<uint[]>
			{
				new uint[] { 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
				new uint[] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 }
			};
		}
	}

	public static bool IsInBounds(uint x, uint y, uint width, uint height)
	{
		return Display.IsInBounds(X, x, Y, y, width, height);
	}

	public static void Draw()
	{
		if (!HardwareCursor)
		{
			for (var x = 0; x < 20; x++)
				for (var y = 0; y < 20; y++)
				{
					var color = mouseCols[y][x];

					if (color != 0)
						Display.DrawPoint((uint)(X + x), (uint)(Y + y), Color);
				}
		}
		else Display.Driver.SetCursor(true, X, Y);
	}
}