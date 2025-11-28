using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

public struct Vector2 : IEquatable<Vector2>, IFormattable
{
	public float X;

	public float Y;

	public static Vector2 One
	{
		get
		{
			throw null;
		}
	}

	public static Vector2 UnitX
	{
		get
		{
			throw null;
		}
	}

	public static Vector2 UnitY
	{
		get
		{
			throw null;
		}
	}

	public float this[int index]
	{
		readonly get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public static Vector2 Zero
	{
		get
		{
			throw null;
		}
	}

	public Vector2(float value)
	{
		throw null;
	}

	public Vector2(float x, float y)
	{
		throw null;
	}

	public Vector2(ReadOnlySpan<float> values)
	{
		throw null;
	}

	public static Vector2 Abs(Vector2 value)
	{
		throw null;
	}

	public static Vector2 Add(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
	{
		throw null;
	}

	public readonly void CopyTo(float[] array)
	{
	}

	public readonly void CopyTo(float[] array, int index)
	{
	}

	public readonly void CopyTo(Span<float> destination)
	{
	}

	public readonly bool TryCopyTo(Span<float> destination)
	{
		throw null;
	}

	public static float Distance(Vector2 value1, Vector2 value2)
	{
		throw null;
	}

	public static float DistanceSquared(Vector2 value1, Vector2 value2)
	{
		throw null;
	}

	public static Vector2 Divide(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 Divide(Vector2 left, float divisor)
	{
		throw null;
	}

	public static float Dot(Vector2 value1, Vector2 value2)
	{
		throw null;
	}

	public readonly bool Equals(Vector2 other)
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

	public readonly float Length()
	{
		throw null;
	}

	public readonly float LengthSquared()
	{
		throw null;
	}

	public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
	{
		throw null;
	}

	public static Vector2 Max(Vector2 value1, Vector2 value2)
	{
		throw null;
	}

	public static Vector2 Min(Vector2 value1, Vector2 value2)
	{
		throw null;
	}

	public static Vector2 Multiply(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 Multiply(Vector2 left, float right)
	{
		throw null;
	}

	public static Vector2 Multiply(float left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 Negate(Vector2 value)
	{
		throw null;
	}

	public static Vector2 Normalize(Vector2 value)
	{
		throw null;
	}

	public static Vector2 operator +(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 operator /(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 operator /(Vector2 value1, float value2)
	{
		throw null;
	}

	public static bool operator ==(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static bool operator !=(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 operator *(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 operator *(Vector2 left, float right)
	{
		throw null;
	}

	public static Vector2 operator *(float left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 operator -(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public static Vector2 operator -(Vector2 value)
	{
		throw null;
	}

	public static Vector2 Reflect(Vector2 vector, Vector2 normal)
	{
		throw null;
	}

	public static Vector2 SquareRoot(Vector2 value)
	{
		throw null;
	}

	public static Vector2 Subtract(Vector2 left, Vector2 right)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public readonly string ToString([StringSyntax("NumericFormat")] string? format)
	{
		throw null;
	}

	public readonly string ToString([StringSyntax("NumericFormat")] string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public static Vector2 Transform(Vector2 position, Matrix3x2 matrix)
	{
		throw null;
	}

	public static Vector2 Transform(Vector2 position, Matrix4x4 matrix)
	{
		throw null;
	}

	public static Vector2 Transform(Vector2 value, Quaternion rotation)
	{
		throw null;
	}

	public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix)
	{
		throw null;
	}

	public static Vector2 TransformNormal(Vector2 normal, Matrix4x4 matrix)
	{
		throw null;
	}
}
