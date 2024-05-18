using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Drawing;

[TypeConverter("System.Drawing.RectangleConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public struct Rectangle : IEquatable<Rectangle>
{
	private int _dummyPrimitive;

	public static readonly Rectangle Empty;

	[Browsable(false)]
	public readonly int Bottom
	{
		get
		{
			throw null;
		}
	}

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

	[Browsable(false)]
	public readonly int Left
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public Point Location
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
	public readonly int Right
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public Size Size
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
	public readonly int Top
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

	public Rectangle(Point location, Size size)
	{
		throw null;
	}

	public Rectangle(int x, int y, int width, int height)
	{
		throw null;
	}

	public static Rectangle Ceiling(RectangleF value)
	{
		throw null;
	}

	public readonly bool Contains(Point pt)
	{
		throw null;
	}

	public readonly bool Contains(Rectangle rect)
	{
		throw null;
	}

	public readonly bool Contains(int x, int y)
	{
		throw null;
	}

	public readonly bool Equals(Rectangle other)
	{
		throw null;
	}

	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static Rectangle FromLTRB(int left, int top, int right, int bottom)
	{
		throw null;
	}

	public override readonly int GetHashCode()
	{
		throw null;
	}

	public static Rectangle Inflate(Rectangle rect, int x, int y)
	{
		throw null;
	}

	public void Inflate(Size size)
	{
	}

	public void Inflate(int width, int height)
	{
	}

	public void Intersect(Rectangle rect)
	{
	}

	public static Rectangle Intersect(Rectangle a, Rectangle b)
	{
		throw null;
	}

	public readonly bool IntersectsWith(Rectangle rect)
	{
		throw null;
	}

	public void Offset(Point pos)
	{
	}

	public void Offset(int x, int y)
	{
	}

	public static bool operator ==(Rectangle left, Rectangle right)
	{
		throw null;
	}

	public static bool operator !=(Rectangle left, Rectangle right)
	{
		throw null;
	}

	public static Rectangle Round(RectangleF value)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public static Rectangle Truncate(RectangleF value)
	{
		throw null;
	}

	public static Rectangle Union(Rectangle a, Rectangle b)
	{
		throw null;
	}
}
