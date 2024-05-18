using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace System.Drawing;

[TypeConverter("System.Drawing.SizeFConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public struct SizeF : IEquatable<SizeF>
{
	private int _dummyPrimitive;

	public static readonly SizeF Empty;

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

	public SizeF(PointF pt)
	{
		throw null;
	}

	public SizeF(SizeF size)
	{
		throw null;
	}

	public SizeF(float width, float height)
	{
		throw null;
	}

	public SizeF(Vector2 vector)
	{
		throw null;
	}

	public static SizeF Add(SizeF sz1, SizeF sz2)
	{
		throw null;
	}

	public readonly bool Equals(SizeF other)
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

	public static explicit operator Vector2(SizeF size)
	{
		throw null;
	}

	public static explicit operator SizeF(Vector2 vector)
	{
		throw null;
	}

	public static SizeF operator +(SizeF sz1, SizeF sz2)
	{
		throw null;
	}

	public static SizeF operator /(SizeF left, float right)
	{
		throw null;
	}

	public static bool operator ==(SizeF sz1, SizeF sz2)
	{
		throw null;
	}

	public static explicit operator PointF(SizeF size)
	{
		throw null;
	}

	public static bool operator !=(SizeF sz1, SizeF sz2)
	{
		throw null;
	}

	public static SizeF operator *(SizeF left, float right)
	{
		throw null;
	}

	public static SizeF operator *(float left, SizeF right)
	{
		throw null;
	}

	public static SizeF operator -(SizeF sz1, SizeF sz2)
	{
		throw null;
	}

	public static SizeF Subtract(SizeF sz1, SizeF sz2)
	{
		throw null;
	}

	public readonly PointF ToPointF()
	{
		throw null;
	}

	public readonly Size ToSize()
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
