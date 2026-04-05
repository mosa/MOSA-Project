using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Drawing;

[Editor("System.Drawing.Design.ColorEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[TypeConverter("System.Drawing.ColorConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public readonly struct Color : IEquatable<Color>
{
	public static readonly Color Empty = new();

	public byte A { get; }

	public byte R { get; }

	public byte G { get; }

	public byte B { get; }

	public string Name => throw new NotImplementedException();

	public bool IsEmpty => !_validColor;

	public bool IsKnownColor => throw new NotImplementedException();

	public bool IsNamedColor => throw new NotImplementedException();

	public bool IsSystemColor => throw new NotImplementedException();

	public static Color AliceBlue { get; } = FromArgb(240, 248, 255);
	public static Color AntiqueWhite { get; } = FromArgb(250, 235, 215);
	public static Color Aqua { get; } = FromArgb(0, 255, 255);
	public static Color Aquamarine { get; } = FromArgb(127, 255, 212);
	public static Color Azure { get; } = FromArgb(240, 255, 255);
	public static Color Beige { get; } = FromArgb(245, 245, 220);
	public static Color Bisque { get; } = FromArgb(255, 228, 196);
	public static Color Black { get; } = FromArgb(0, 0, 0);
	public static Color BlanchedAlmond { get; } = FromArgb(255, 255, 205);
	public static Color Blue { get; } = FromArgb(0, 0, 255);
	public static Color BlueViolet { get; } = FromArgb(138, 43, 226);
	public static Color Brown { get; } = FromArgb(165, 42, 42);
	public static Color BurlyWood { get; } = FromArgb(222, 184, 135);
	public static Color CadetBlue { get; } = FromArgb(95, 158, 160);
	public static Color Chartreuse { get; } = FromArgb(127, 255, 0);
	public static Color Chocolate { get; } = FromArgb(210, 105, 30);
	public static Color Coral { get; } = FromArgb(255, 127, 80);
	public static Color CornflowerBlue { get; } = FromArgb(100, 149, 237);
	public static Color Cornsilk { get; } = FromArgb(255, 248, 220);
	public static Color Crimson { get; } = FromArgb(220, 20, 60);
	public static Color Cyan { get; } = FromArgb(0, 255, 255);
	public static Color DarkBlue { get; } = FromArgb(0, 0, 139);
	public static Color DarkCyan { get; } = FromArgb(0, 139, 139);
	public static Color DarkGoldenrod { get; } = FromArgb(184, 134, 11);
	public static Color DarkGray { get; } = FromArgb(169, 169, 169);
	public static Color DarkGreen { get; } = FromArgb(0, 100, 0);
	public static Color DarkKhaki { get; } = FromArgb(189, 183, 107);
	public static Color DarkMagenta { get; } = FromArgb(139, 0, 139);
	public static Color DarkOliveGreen { get; } = FromArgb(85, 107, 47);
	public static Color DarkOrange { get; } = FromArgb(255, 140, 0);
	public static Color DarkOrchid { get; } = FromArgb(153, 50, 204);
	public static Color DarkRed { get; } = FromArgb(139, 0, 0);
	public static Color DarkSalmon { get; } = FromArgb(233, 150, 122);
	public static Color DarkSeaGreen { get; } = FromArgb(143, 188, 143);
	public static Color DarkSlateBlue { get; } = FromArgb(72, 61, 139);
	public static Color DarkSlateGray { get; } = FromArgb(40, 79, 79);
	public static Color DarkTurquoise { get; } = FromArgb(0, 206, 209);
	public static Color DarkViolet { get; } = FromArgb(148, 0, 211);
	public static Color DeepPink { get; } = FromArgb(255, 20, 147);
	public static Color DeepSkyBlue { get; } = FromArgb(0, 191, 255);
	public static Color DimGray { get; } = FromArgb(105, 105, 105);
	public static Color DodgerBlue { get; } = FromArgb(30, 144, 255);
	public static Color Firebrick { get; } = FromArgb(178, 34, 34);
	public static Color FloralWhite { get; } = FromArgb(255, 250, 240);
	public static Color ForestGreen { get; } = FromArgb(34, 139, 34);
	public static Color Fuchsia { get; } = FromArgb(255, 0, 255);
	public static Color Gainsboro { get; } = FromArgb(220, 220, 220);
	public static Color GhostWhite { get; } = FromArgb(248, 248, 255);
	public static Color Gold { get; } = FromArgb(255, 215, 0);
	public static Color Goldenrod { get; } = FromArgb(218, 165, 32);
	public static Color Gray { get; } = FromArgb(128, 128, 128);
	public static Color Green { get; } = FromArgb(0, 128, 0);
	public static Color GreenYellow { get; } = FromArgb(173, 255, 47);
	public static Color Honeydew { get; } = FromArgb(240, 255, 240);
	public static Color HotPink { get; } = FromArgb(255, 105, 180);
	public static Color IndianRed { get; } = FromArgb(205, 92, 92);
	public static Color Indigo { get; } = FromArgb(75, 0, 130);
	public static Color Ivory { get; } = FromArgb(255, 240, 240);
	public static Color Khaki { get; } = FromArgb(240, 230, 140);
	public static Color Lavender { get; } = FromArgb(230, 230, 250);
	public static Color LavenderBlush { get; } = FromArgb(255, 240, 245);
	public static Color LawnGreen { get; } = FromArgb(124, 252, 0);
	public static Color LemonChiffon { get; } = FromArgb(255, 250, 205);
	public static Color LightBlue { get; } = FromArgb(173, 216, 230);
	public static Color LightCoral { get; } = FromArgb(240, 128, 128);
	public static Color LightCyan { get; } = FromArgb(224, 255, 255);
	public static Color LightGoldenrodYellow { get; } = FromArgb(250, 250, 210);
	public static Color LightGray { get; } = FromArgb(211, 211, 211);
	public static Color LightGreen { get; } = FromArgb(144, 238, 144);
	public static Color LightPink { get; } = FromArgb(255, 182, 193);
	public static Color LightSalmon { get; } = FromArgb(255, 160, 122);
	public static Color LightSeaGreen { get; } = FromArgb(32, 178, 170);
	public static Color LightSkyBlue { get; } = FromArgb(135, 206, 250);
	public static Color LightSlateGray { get; } = FromArgb(119, 136, 153);
	public static Color LightSteelBlue { get; } = FromArgb(176, 196, 222);
	public static Color LightYellow { get; } = FromArgb(255, 255, 224);
	public static Color Lime { get; } = FromArgb(0, 255, 0);
	public static Color LimeGreen { get; } = FromArgb(50, 205, 50);
	public static Color Linen { get; } = FromArgb(250, 240, 230);
	public static Color Magenta { get; } = FromArgb(255, 0, 255);
	public static Color Maroon { get; } = FromArgb(128, 0, 0);
	public static Color MediumAquamarine { get; } = FromArgb(102, 205, 170);
	public static Color MediumBlue { get; } = FromArgb(0, 0, 205);
	public static Color MediumOrchid { get; } = FromArgb(186, 85, 211);
	public static Color MediumPurple { get; } = FromArgb(147, 112, 219);
	public static Color MediumSeaGreen { get; } = FromArgb(60, 179, 113);
	public static Color MediumSlateBlue { get; } = FromArgb(123, 104, 238);
	public static Color MediumSpringGreen { get; } = FromArgb(0, 250, 154);
	public static Color MediumTurquoise { get; } = FromArgb(72, 209, 204);
	public static Color MediumVioletRed { get; } = FromArgb(199, 21, 112);
	public static Color MidnightBlue { get; } = FromArgb(25, 25, 112);
	public static Color MintCream { get; } = FromArgb(245, 255, 250);
	public static Color MistyRose { get; } = FromArgb(255, 228, 225);
	public static Color Moccasin { get; } = FromArgb(255, 228, 181);
	public static Color NavajoWhite { get; } = FromArgb(255, 222, 173);
	public static Color Navy { get; } = FromArgb(0, 0, 128);
	public static Color OldLace { get; } = FromArgb(253, 245, 230);
	public static Color Olive { get; } = FromArgb(128, 128, 0);
	public static Color OliveDrab { get; } = FromArgb(107, 142, 45);
	public static Color Orange { get; } = FromArgb(255, 165, 0);
	public static Color OrangeRed { get; } = FromArgb(255, 69, 0);
	public static Color Orchid { get; } = FromArgb(218, 112, 214);
	public static Color PaleGoldenrod { get; } = FromArgb(238, 232, 170);
	public static Color PaleGreen { get; } = FromArgb(152, 251, 152);
	public static Color PaleTurquoise { get; } = FromArgb(175, 238, 238);
	public static Color PaleVioletRed { get; } = FromArgb(219, 112, 147);
	public static Color PapayaWhip { get; } = FromArgb(255, 239, 213);
	public static Color PeachPuff { get; } = FromArgb(255, 218, 155);
	public static Color Peru { get; } = FromArgb(205, 133, 63);
	public static Color Pink { get; } = FromArgb(255, 192, 203);
	public static Color Plum { get; } = FromArgb(221, 160, 221);
	public static Color PowderBlue { get; } = FromArgb(176, 224, 230);
	public static Color Purple { get; } = FromArgb(128, 0, 128);
	public static Color RebeccaPurple => FromArgb(102, 51, 153);
	public static Color Red { get; } = FromArgb(255, 0, 0);
	public static Color RosyBrown { get; } = FromArgb(188, 143, 143);
	public static Color RoyalBlue { get; } = FromArgb(65, 105, 225);
	public static Color SaddleBrown { get; } = FromArgb(139, 69, 19);
	public static Color Salmon { get; } = FromArgb(250, 128, 114);
	public static Color SandyBrown { get; } = FromArgb(244, 164, 96);
	public static Color SeaGreen { get; } = FromArgb(46, 139, 87);
	public static Color SeaShell { get; } = FromArgb(255, 245, 238);
	public static Color Sienna { get; } = FromArgb(160, 82, 45);
	public static Color Silver { get; } = FromArgb(192, 192, 192);
	public static Color SkyBlue { get; } = FromArgb(135, 206, 235);
	public static Color SlateBlue { get; } = FromArgb(106, 90, 205);
	public static Color SlateGray { get; } = FromArgb(112, 128, 144);
	public static Color Snow { get; } = FromArgb(255, 250, 250);
	public static Color SpringGreen { get; } = FromArgb(0, 255, 127);
	public static Color SteelBlue { get; } = FromArgb(70, 130, 180);
	public static Color Tan { get; } = FromArgb(210, 180, 140);
	public static Color Teal { get; } = FromArgb(0, 128, 128);
	public static Color Thistle { get; } = FromArgb(216, 191, 216);
	public static Color Tomato { get; } = FromArgb(253, 99, 71);
	public static Color Transparent { get; } = FromArgb(0, 0, 0, 0);
	public static Color Turquoise { get; } = FromArgb(64, 224, 208);
	public static Color Violet { get; } = FromArgb(238, 130, 238);
	public static Color Wheat { get; } = FromArgb(245, 222, 179);
	public static Color White { get; } = FromArgb(255, 255, 255);
	public static Color WhiteSmoke { get; } = FromArgb(245, 245, 245);
	public static Color Yellow { get; } = FromArgb(255, 255, 0);
	public static Color YellowGreen { get; } = FromArgb(154, 205, 50);

	private readonly int _argb;
	private readonly bool _validColor;

	private Color(int argb)
	{
		A = (byte)((argb >> 24) & 0xFF);
		R = (byte)((argb >> 16) & 0xFF);
		G = (byte)((argb >> 8) & 0xFF);
		B = (byte)(argb & 0xFF);
		_argb = argb;
		_validColor = true;
	}

	private Color(byte a, byte r, byte g, byte b)
	{
		A = a;
		R = r;
		G = g;
		B = b;
		_argb = a << 24 | r << 16 | g << 8 | b;
		_validColor = true;
	}

	public static bool operator ==(Color left, Color right) => left.Equals(right);

	public static bool operator !=(Color left, Color right) => !left.Equals(right);

	public bool Equals(Color other) => A == other.A && R == other.R && G == other.G && B == other.B;

	public override bool Equals([NotNullWhen(true)] object? obj) => obj is Color other && Equals(other);

	public override int GetHashCode() => HashCode.Combine(A, R, G, B);

	public override string ToString() => throw new NotImplementedException();

	public static Color FromArgb(int argb) => new(argb);

	public static Color FromArgb(int alpha, Color baseColor)
	{
		if (alpha < 0 || alpha > 255)
			Internal.Exceptions.Color.ThrowComponentOutOfRangeException(nameof(alpha));

		return new((byte)alpha, baseColor.R, baseColor.G, baseColor.B);
	}

	public static Color FromArgb(int red, int green, int blue)
	{
		if (red < 0 || red > 255)
			Internal.Exceptions.Color.ThrowComponentOutOfRangeException(nameof(red));
		if (green < 0 || green > 255)
			Internal.Exceptions.Color.ThrowComponentOutOfRangeException(nameof(green));
		if (blue < 0 || blue > 255)
			Internal.Exceptions.Color.ThrowComponentOutOfRangeException(nameof(blue));

		return new(255, (byte)red, (byte)green, (byte)blue);
	}

	public static Color FromArgb(int alpha, int red, int green, int blue) =>
		new((byte)alpha, (byte)red, (byte)green, (byte)blue);

	public int ToArgb() => _argb;

	public static Color FromKnownColor(KnownColor color) => throw new NotImplementedException();

	public KnownColor ToKnownColor() => throw new NotImplementedException();

	public static Color FromName(string name) => throw new NotImplementedException();

	public float GetBrightness() => throw new NotImplementedException();

	public float GetHue() => throw new NotImplementedException();

	public float GetSaturation() => throw new NotImplementedException();
}
