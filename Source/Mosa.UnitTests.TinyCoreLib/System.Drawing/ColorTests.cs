// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;

namespace Mosa.UnitTests.TinyCoreLib;

public static class ColorTests
{
	// == FromArgb component test

	[MosaUnitTest(Series = "U1U1U1U1")]
	public static bool Test_Color_FromArgb_ARGB(byte alpha, byte red, byte green, byte blue)
	{
		var color = Color.FromArgb(alpha, red, green, blue);
		return color.A == alpha && color.R == red && color.G == green && color.B == blue;
	}

	[MosaUnitTest(Series = "U1U1U1")]
	public static bool Test_Color_FromArgb_RGB(byte red, byte green, byte blue)
	{
		var color = Color.FromArgb(red, green, blue);
		return color.A == 255 && color.R == red && color.G == green && color.B == blue;
	}

	[MosaUnitTest(Series = "U1U1U1U1")]
	public static bool Test_Color_FromArgb_AlphaAndColor(byte alpha, byte red, byte green, byte blue)
	{
		var baseColor = Color.FromArgb(red, green, blue);
		var color = Color.FromArgb(alpha, baseColor);

		return color.A == alpha && color.R == red && color.G == green && color.B == blue;
	}

	// == Known color test

	[MosaUnitTest]
	public static bool Test_Color_KnownColors_Red()
	{
		var red = Color.Red;
		return red.A == 255 && red.R == 255 && red.G == 0 && red.B == 0;
	}

	[MosaUnitTest]
	public static bool Test_Color_KnownColors_Green()
	{
		var green = Color.Green;
		return green.A == 255 && green.R == 0 && green.G == 128 && green.B == 0;
	}

	[MosaUnitTest]
	public static bool Test_Color_KnownColors_Blue()
	{
		var blue = Color.Blue;
		return blue.A == 255 && blue.R == 0 && blue.G == 0 && blue.B == 255;
	}

	[MosaUnitTest]
	public static bool Test_Color_KnownColors_White()
	{
		var white = Color.White;
		return white.A == 255 && white.R == 255 && white.G == 255 && white.B == 255;
	}

	[MosaUnitTest]
	public static bool Test_Color_KnownColors_Black()
	{
		var black = Color.Black;
		return black.A == 255 && black.R == 0 && black.G == 0 && black.B == 0;
	}

	// == ToArgb tests

	[MosaUnitTest(Series = "U1U1U1U1")]
	public static int Test_Color_ToArgb(byte alpha, byte red, byte green, byte blue)
	{
		var color = Color.FromArgb(alpha, red, green, blue);
		return color.ToArgb();
	}

	[MosaUnitTest(Series = "I4")]
	public static int Test_Color_FromArgbValue(int argb)
	{
		var color = Color.FromArgb(argb);
		return color.ToArgb();
	}

	// == Equality tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Color_Equality(int argb)
	{
		var color1 = Color.FromArgb(argb);
		var color2 = Color.FromArgb(argb);
		var color3 = Color.FromArgb(argb + 1);

		return color1 == color2 && color1 != color3;
	}
}
