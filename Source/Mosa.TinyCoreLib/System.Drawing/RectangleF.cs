using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace System.Drawing;

public struct RectangleF : IEquatable<RectangleF>
{
	private int _dummyPrimitive;

	public static readonly RectangleF Empty;

	[Browsable(false)]
	public readonly float Bottom
	{
		get
		{
			throw null;
		}
	}

	public float Height
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
	public readonly float Left
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public PointF Location
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
	public readonly float Right
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public SizeF Size
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
	public readonly float Top
	{
		get
		{
			throw null;
		}
	}

	public float Width
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public float X
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public float Y
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public RectangleF(PointF location, SizeF size)
	{
		throw null;
	}

	public RectangleF(float x, float y, float width, float height)
	{
		throw null;
	}

	public RectangleF(Vector4 vector)
	{
		throw null;
	}

	public readonly bool Contains(PointF pt)
	{
		throw null;
	}

	public readonly bool Contains(RectangleF rect)
	{
		throw null;
	}

	public readonly bool Contains(float x, float y)
	{
		throw null;
	}

	public readonly bool Equals(RectangleF other)
	{
		throw null;
	}

	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static RectangleF FromLTRB(float left, float top, float right, float bottom)
	{
		throw null;
	}

	public override readonly int GetHashCode()
	{
		throw null;
	}

	public static RectangleF Inflate(RectangleF rect, float x, float y)
	{
		throw null;
	}

	public void Inflate(SizeF size)
	{
	}

	public void Inflate(float x, float y)
	{
	}

	public void Intersect(RectangleF rect)
	{
	}

	public static RectangleF Intersect(RectangleF a, RectangleF b)
	{
		throw null;
	}

	public readonly bool IntersectsWith(RectangleF rect)
	{
		throw null;
	}

	public void Offset(PointF pos)
	{
	}

	public void Offset(float x, float y)
	{
	}

	public static explicit operator Vector4(RectangleF rectangle)
	{
		throw null;
	}

	public static explicit operator RectangleF(Vector4 vector)
	{
		throw null;
	}

	public static bool operator ==(RectangleF left, RectangleF right)
	{
		throw null;
	}

	public static implicit operator RectangleF(Rectangle r)
	{
		throw null;
	}

	public static bool operator !=(RectangleF left, RectangleF right)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public Vector4 ToVector4()
	{
		throw null;
	}

	public static RectangleF Union(RectangleF a, RectangleF b)
	{
		throw null;
	}
}
