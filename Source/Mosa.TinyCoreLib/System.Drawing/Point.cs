using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Drawing;

[TypeConverter("System.Drawing.PointConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public struct Point : IEquatable<Point>
{
	private int _dummyPrimitive;

	public static readonly Point Empty;

	[Browsable(false)]
	public readonly bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public int X
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Y
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public Point(Size sz)
	{
		throw null;
	}

	public Point(int dw)
	{
		throw null;
	}

	public Point(int x, int y)
	{
		throw null;
	}

	public static Point Add(Point pt, Size sz)
	{
		throw null;
	}

	public static Point Ceiling(PointF value)
	{
		throw null;
	}

	public readonly bool Equals(Point other)
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

	public void Offset(Point p)
	{
	}

	public void Offset(int dx, int dy)
	{
	}

	public static Point operator +(Point pt, Size sz)
	{
		throw null;
	}

	public static bool operator ==(Point left, Point right)
	{
		throw null;
	}

	public static explicit operator Size(Point p)
	{
		throw null;
	}

	public static implicit operator PointF(Point p)
	{
		throw null;
	}

	public static bool operator !=(Point left, Point right)
	{
		throw null;
	}

	public static Point operator -(Point pt, Size sz)
	{
		throw null;
	}

	public static Point Round(PointF value)
	{
		throw null;
	}

	public static Point Subtract(Point pt, Size sz)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public static Point Truncate(PointF value)
	{
		throw null;
	}
}
