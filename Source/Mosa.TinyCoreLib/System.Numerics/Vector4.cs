using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

public struct Vector4 : IEquatable<Vector4>, IFormattable
{
	public float X;

	public float Y;

	public float Z;

	public float W;

	public static Vector4 One
	{
		get
		{
			throw null;
		}
	}

	public static Vector4 UnitW
	{
		get
		{
			throw null;
		}
	}

	public static Vector4 UnitX
	{
		get
		{
			throw null;
		}
	}

	public static Vector4 UnitY
	{
		get
		{
			throw null;
		}
	}

	public static Vector4 UnitZ
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

	public static Vector4 Zero
	{
		get
		{
			throw null;
		}
	}

	public Vector4(Vector2 value, float z, float w)
	{
		throw null;
	}

	public Vector4(Vector3 value, float w)
	{
		throw null;
	}

	public Vector4(float value)
	{
		throw null;
	}

	public Vector4(float x, float y, float z, float w)
	{
		throw null;
	}

	public Vector4(ReadOnlySpan<float> values)
	{
		throw null;
	}

	public static Vector4 Abs(Vector4 value)
	{
		throw null;
	}

	public static Vector4 Add(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max)
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

	public static float Distance(Vector4 value1, Vector4 value2)
	{
		throw null;
	}

	public static float DistanceSquared(Vector4 value1, Vector4 value2)
	{
		throw null;
	}

	public static Vector4 Divide(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 Divide(Vector4 left, float divisor)
	{
		throw null;
	}

	public static float Dot(Vector4 vector1, Vector4 vector2)
	{
		throw null;
	}

	public readonly bool Equals(Vector4 other)
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

	public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount)
	{
		throw null;
	}

	public static Vector4 Max(Vector4 value1, Vector4 value2)
	{
		throw null;
	}

	public static Vector4 Min(Vector4 value1, Vector4 value2)
	{
		throw null;
	}

	public static Vector4 Multiply(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 Multiply(Vector4 left, float right)
	{
		throw null;
	}

	public static Vector4 Multiply(float left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 Negate(Vector4 value)
	{
		throw null;
	}

	public static Vector4 Normalize(Vector4 vector)
	{
		throw null;
	}

	public static Vector4 operator +(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 operator /(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 operator /(Vector4 value1, float value2)
	{
		throw null;
	}

	public static bool operator ==(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static bool operator !=(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 operator *(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 operator *(Vector4 left, float right)
	{
		throw null;
	}

	public static Vector4 operator *(float left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 operator -(Vector4 left, Vector4 right)
	{
		throw null;
	}

	public static Vector4 operator -(Vector4 value)
	{
		throw null;
	}

	public static Vector4 SquareRoot(Vector4 value)
	{
		throw null;
	}

	public static Vector4 Subtract(Vector4 left, Vector4 right)
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

	public static Vector4 Transform(Vector2 position, Matrix4x4 matrix)
	{
		throw null;
	}

	public static Vector4 Transform(Vector2 value, Quaternion rotation)
	{
		throw null;
	}

	public static Vector4 Transform(Vector3 position, Matrix4x4 matrix)
	{
		throw null;
	}

	public static Vector4 Transform(Vector3 value, Quaternion rotation)
	{
		throw null;
	}

	public static Vector4 Transform(Vector4 vector, Matrix4x4 matrix)
	{
		throw null;
	}

	public static Vector4 Transform(Vector4 value, Quaternion rotation)
	{
		throw null;
	}
}
