using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Drawing;

[Editor("System.Drawing.Design.ColorEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[TypeConverter("System.Drawing.ColorConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public readonly struct Color : IEquatable<Color>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static readonly Color Empty;

	public byte A
	{
		get
		{
			throw null;
		}
	}

	public static Color AliceBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color AntiqueWhite
	{
		get
		{
			throw null;
		}
	}

	public static Color Aqua
	{
		get
		{
			throw null;
		}
	}

	public static Color Aquamarine
	{
		get
		{
			throw null;
		}
	}

	public static Color Azure
	{
		get
		{
			throw null;
		}
	}

	public byte B
	{
		get
		{
			throw null;
		}
	}

	public static Color Beige
	{
		get
		{
			throw null;
		}
	}

	public static Color Bisque
	{
		get
		{
			throw null;
		}
	}

	public static Color Black
	{
		get
		{
			throw null;
		}
	}

	public static Color BlanchedAlmond
	{
		get
		{
			throw null;
		}
	}

	public static Color Blue
	{
		get
		{
			throw null;
		}
	}

	public static Color BlueViolet
	{
		get
		{
			throw null;
		}
	}

	public static Color Brown
	{
		get
		{
			throw null;
		}
	}

	public static Color BurlyWood
	{
		get
		{
			throw null;
		}
	}

	public static Color CadetBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color Chartreuse
	{
		get
		{
			throw null;
		}
	}

	public static Color Chocolate
	{
		get
		{
			throw null;
		}
	}

	public static Color Coral
	{
		get
		{
			throw null;
		}
	}

	public static Color CornflowerBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color Cornsilk
	{
		get
		{
			throw null;
		}
	}

	public static Color Crimson
	{
		get
		{
			throw null;
		}
	}

	public static Color Cyan
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkCyan
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkGoldenrod
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkGray
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkKhaki
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkMagenta
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkOliveGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkOrange
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkOrchid
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkRed
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkSalmon
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkSeaGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkSlateBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkSlateGray
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkTurquoise
	{
		get
		{
			throw null;
		}
	}

	public static Color DarkViolet
	{
		get
		{
			throw null;
		}
	}

	public static Color DeepPink
	{
		get
		{
			throw null;
		}
	}

	public static Color DeepSkyBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color DimGray
	{
		get
		{
			throw null;
		}
	}

	public static Color DodgerBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color Firebrick
	{
		get
		{
			throw null;
		}
	}

	public static Color FloralWhite
	{
		get
		{
			throw null;
		}
	}

	public static Color ForestGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color Fuchsia
	{
		get
		{
			throw null;
		}
	}

	public byte G
	{
		get
		{
			throw null;
		}
	}

	public static Color Gainsboro
	{
		get
		{
			throw null;
		}
	}

	public static Color GhostWhite
	{
		get
		{
			throw null;
		}
	}

	public static Color Gold
	{
		get
		{
			throw null;
		}
	}

	public static Color Goldenrod
	{
		get
		{
			throw null;
		}
	}

	public static Color Gray
	{
		get
		{
			throw null;
		}
	}

	public static Color Green
	{
		get
		{
			throw null;
		}
	}

	public static Color GreenYellow
	{
		get
		{
			throw null;
		}
	}

	public static Color Honeydew
	{
		get
		{
			throw null;
		}
	}

	public static Color HotPink
	{
		get
		{
			throw null;
		}
	}

	public static Color IndianRed
	{
		get
		{
			throw null;
		}
	}

	public static Color Indigo
	{
		get
		{
			throw null;
		}
	}

	public bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public bool IsKnownColor
	{
		get
		{
			throw null;
		}
	}

	public bool IsNamedColor
	{
		get
		{
			throw null;
		}
	}

	public bool IsSystemColor
	{
		get
		{
			throw null;
		}
	}

	public static Color Ivory
	{
		get
		{
			throw null;
		}
	}

	public static Color Khaki
	{
		get
		{
			throw null;
		}
	}

	public static Color Lavender
	{
		get
		{
			throw null;
		}
	}

	public static Color LavenderBlush
	{
		get
		{
			throw null;
		}
	}

	public static Color LawnGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color LemonChiffon
	{
		get
		{
			throw null;
		}
	}

	public static Color LightBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color LightCoral
	{
		get
		{
			throw null;
		}
	}

	public static Color LightCyan
	{
		get
		{
			throw null;
		}
	}

	public static Color LightGoldenrodYellow
	{
		get
		{
			throw null;
		}
	}

	public static Color LightGray
	{
		get
		{
			throw null;
		}
	}

	public static Color LightGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color LightPink
	{
		get
		{
			throw null;
		}
	}

	public static Color LightSalmon
	{
		get
		{
			throw null;
		}
	}

	public static Color LightSeaGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color LightSkyBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color LightSlateGray
	{
		get
		{
			throw null;
		}
	}

	public static Color LightSteelBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color LightYellow
	{
		get
		{
			throw null;
		}
	}

	public static Color Lime
	{
		get
		{
			throw null;
		}
	}

	public static Color LimeGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color Linen
	{
		get
		{
			throw null;
		}
	}

	public static Color Magenta
	{
		get
		{
			throw null;
		}
	}

	public static Color Maroon
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumAquamarine
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumOrchid
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumPurple
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumSeaGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumSlateBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumSpringGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumTurquoise
	{
		get
		{
			throw null;
		}
	}

	public static Color MediumVioletRed
	{
		get
		{
			throw null;
		}
	}

	public static Color MidnightBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color MintCream
	{
		get
		{
			throw null;
		}
	}

	public static Color MistyRose
	{
		get
		{
			throw null;
		}
	}

	public static Color Moccasin
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public static Color NavajoWhite
	{
		get
		{
			throw null;
		}
	}

	public static Color Navy
	{
		get
		{
			throw null;
		}
	}

	public static Color OldLace
	{
		get
		{
			throw null;
		}
	}

	public static Color Olive
	{
		get
		{
			throw null;
		}
	}

	public static Color OliveDrab
	{
		get
		{
			throw null;
		}
	}

	public static Color Orange
	{
		get
		{
			throw null;
		}
	}

	public static Color OrangeRed
	{
		get
		{
			throw null;
		}
	}

	public static Color Orchid
	{
		get
		{
			throw null;
		}
	}

	public static Color PaleGoldenrod
	{
		get
		{
			throw null;
		}
	}

	public static Color PaleGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color PaleTurquoise
	{
		get
		{
			throw null;
		}
	}

	public static Color PaleVioletRed
	{
		get
		{
			throw null;
		}
	}

	public static Color PapayaWhip
	{
		get
		{
			throw null;
		}
	}

	public static Color PeachPuff
	{
		get
		{
			throw null;
		}
	}

	public static Color Peru
	{
		get
		{
			throw null;
		}
	}

	public static Color Pink
	{
		get
		{
			throw null;
		}
	}

	public static Color Plum
	{
		get
		{
			throw null;
		}
	}

	public static Color PowderBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color Purple
	{
		get
		{
			throw null;
		}
	}

	public byte R
	{
		get
		{
			throw null;
		}
	}

	public static Color RebeccaPurple
	{
		get
		{
			throw null;
		}
	}

	public static Color Red
	{
		get
		{
			throw null;
		}
	}

	public static Color RosyBrown
	{
		get
		{
			throw null;
		}
	}

	public static Color RoyalBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color SaddleBrown
	{
		get
		{
			throw null;
		}
	}

	public static Color Salmon
	{
		get
		{
			throw null;
		}
	}

	public static Color SandyBrown
	{
		get
		{
			throw null;
		}
	}

	public static Color SeaGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color SeaShell
	{
		get
		{
			throw null;
		}
	}

	public static Color Sienna
	{
		get
		{
			throw null;
		}
	}

	public static Color Silver
	{
		get
		{
			throw null;
		}
	}

	public static Color SkyBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color SlateBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color SlateGray
	{
		get
		{
			throw null;
		}
	}

	public static Color Snow
	{
		get
		{
			throw null;
		}
	}

	public static Color SpringGreen
	{
		get
		{
			throw null;
		}
	}

	public static Color SteelBlue
	{
		get
		{
			throw null;
		}
	}

	public static Color Tan
	{
		get
		{
			throw null;
		}
	}

	public static Color Teal
	{
		get
		{
			throw null;
		}
	}

	public static Color Thistle
	{
		get
		{
			throw null;
		}
	}

	public static Color Tomato
	{
		get
		{
			throw null;
		}
	}

	public static Color Transparent
	{
		get
		{
			throw null;
		}
	}

	public static Color Turquoise
	{
		get
		{
			throw null;
		}
	}

	public static Color Violet
	{
		get
		{
			throw null;
		}
	}

	public static Color Wheat
	{
		get
		{
			throw null;
		}
	}

	public static Color White
	{
		get
		{
			throw null;
		}
	}

	public static Color WhiteSmoke
	{
		get
		{
			throw null;
		}
	}

	public static Color Yellow
	{
		get
		{
			throw null;
		}
	}

	public static Color YellowGreen
	{
		get
		{
			throw null;
		}
	}

	public bool Equals(Color other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static Color FromArgb(int argb)
	{
		throw null;
	}

	public static Color FromArgb(int alpha, Color baseColor)
	{
		throw null;
	}

	public static Color FromArgb(int red, int green, int blue)
	{
		throw null;
	}

	public static Color FromArgb(int alpha, int red, int green, int blue)
	{
		throw null;
	}

	public static Color FromKnownColor(KnownColor color)
	{
		throw null;
	}

	public static Color FromName(string name)
	{
		throw null;
	}

	public float GetBrightness()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public float GetHue()
	{
		throw null;
	}

	public float GetSaturation()
	{
		throw null;
	}

	public static bool operator ==(Color left, Color right)
	{
		throw null;
	}

	public static bool operator !=(Color left, Color right)
	{
		throw null;
	}

	public int ToArgb()
	{
		throw null;
	}

	public KnownColor ToKnownColor()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
