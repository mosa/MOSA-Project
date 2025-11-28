using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace System.Drawing;

public struct PointF : IEquatable<PointF>
{
	private int _dummyPrimitive;

	public static readonly PointF Empty;

	[Browsable(false)]
	public readonly bool IsEmpty
	{
		get
		{
			throw null;
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

	public PointF(float x, float y)
	{
		throw null;
	}

	public PointF(Vector2 vector)
	{
		throw null;
	}

	public static PointF Add(PointF pt, Size sz)
	{
		throw null;
	}

	public static PointF Add(PointF pt, SizeF sz)
	{
		throw null;
	}

	public readonly bool Equals(PointF other)
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

	public static explicit operator Vector2(PointF point)
	{
		throw null;
	}

	public static explicit operator PointF(Vector2 vector)
	{
		throw null;
	}

	public static PointF operator +(PointF pt, Size sz)
	{
		throw null;
	}

	public static PointF operator +(PointF pt, SizeF sz)
	{
		throw null;
	}

	public static bool operator ==(PointF left, PointF right)
	{
		throw null;
	}

	public static bool operator !=(PointF left, PointF right)
	{
		throw null;
	}

	public static PointF operator -(PointF pt, Size sz)
	{
		throw null;
	}

	public static PointF operator -(PointF pt, SizeF sz)
	{
		throw null;
	}

	public static PointF Subtract(PointF pt, Size sz)
	{
		throw null;
	}

	public static PointF Subtract(PointF pt, SizeF sz)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public Vector2 ToVector2()
	{
		throw null;
	}
}
