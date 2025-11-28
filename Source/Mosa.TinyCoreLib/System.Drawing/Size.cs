using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Drawing;

[TypeConverter("System.Drawing.SizeConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public struct Size : IEquatable<Size>
{
	private int _dummyPrimitive;

	public static readonly Size Empty;

	public int Height
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public readonly bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public int Width
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public Size(Point pt)
	{
		throw null;
	}

	public Size(int width, int height)
	{
		throw null;
	}

	public static Size Add(Size sz1, Size sz2)
	{
		throw null;
	}

	public static Size Ceiling(SizeF value)
	{
		throw null;
	}

	public readonly bool Equals(Size other)
	{
		throw null;
	}

	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override readonly int GetHashCode()
	{
		throw null;
	}

	public static Size operator +(Size sz1, Size sz2)
	{
		throw null;
	}

	public static Size operator /(Size left, int right)
	{
		throw null;
	}

	public static SizeF operator /(Size left, float right)
	{
		throw null;
	}

	public static bool operator ==(Size sz1, Size sz2)
	{
		throw null;
	}

	public static explicit operator Point(Size size)
	{
		throw null;
	}

	public static implicit operator SizeF(Size p)
	{
		throw null;
	}

	public static bool operator !=(Size sz1, Size sz2)
	{
		throw null;
	}

	public static Size operator *(Size left, int right)
	{
		throw null;
	}

	public static SizeF operator *(Size left, float right)
	{
		throw null;
	}

	public static Size operator *(int left, Size right)
	{
		throw null;
	}

	public static SizeF operator *(float left, Size right)
	{
		throw null;
	}

	public static Size operator -(Size sz1, Size sz2)
	{
		throw null;
	}

	public static Size Round(SizeF value)
	{
		throw null;
	}

	public static Size Subtract(Size sz1, Size sz2)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public static Size Truncate(SizeF value)
	{
		throw null;
	}
}
