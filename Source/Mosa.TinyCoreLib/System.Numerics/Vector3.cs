using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

public struct Vector3 : IEquatable<Vector3>, IFormattable
{
	public float X;

	public float Y;

	public float Z;

	public static Vector3 One
	{
		get
		{
			throw null;
		}
	}

	public static Vector3 UnitX
	{
		get
		{
			throw null;
		}
	}

	public static Vector3 UnitY
	{
		get
		{
			throw null;
		}
	}

	public static Vector3 UnitZ
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

	public static Vector3 Zero
	{
		get
		{
			throw null;
		}
	}

	public Vector3(Vector2 value, float z)
	{
		throw null;
	}

	public Vector3(float value)
	{
		throw null;
	}

	public Vector3(float x, float y, float z)
	{
		throw null;
	}

	public Vector3(ReadOnlySpan<float> values)
	{
		throw null;
	}

	public static Vector3 Abs(Vector3 value)
	{
		throw null;
	}

	public static Vector3 Add(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
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

	public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
	{
		throw null;
	}

	public static float Distance(Vector3 value1, Vector3 value2)
	{
		throw null;
	}

	public static float DistanceSquared(Vector3 value1, Vector3 value2)
	{
		throw null;
	}

	public static Vector3 Divide(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 Divide(Vector3 left, float divisor)
	{
		throw null;
	}

	public static float Dot(Vector3 vector1, Vector3 vector2)
	{
		throw null;
	}

	public readonly bool Equals(Vector3 other)
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

	public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
	{
		throw null;
	}

	public static Vector3 Max(Vector3 value1, Vector3 value2)
	{
		throw null;
	}

	public static Vector3 Min(Vector3 value1, Vector3 value2)
	{
		throw null;
	}

	public static Vector3 Multiply(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 Multiply(Vector3 left, float right)
	{
		throw null;
	}

	public static Vector3 Multiply(float left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 Negate(Vector3 value)
	{
		throw null;
	}

	public static Vector3 Normalize(Vector3 value)
	{
		throw null;
	}

	public static Vector3 operator +(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 operator /(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 operator /(Vector3 value1, float value2)
	{
		throw null;
	}

	public static bool operator ==(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static bool operator !=(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 operator *(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 operator *(Vector3 left, float right)
	{
		throw null;
	}

	public static Vector3 operator *(float left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 operator -(Vector3 left, Vector3 right)
	{
		throw null;
	}

	public static Vector3 operator -(Vector3 value)
	{
		throw null;
	}

	public static Vector3 Reflect(Vector3 vector, Vector3 normal)
	{
		throw null;
	}

	public static Vector3 SquareRoot(Vector3 value)
	{
		throw null;
	}

	public static Vector3 Subtract(Vector3 left, Vector3 right)
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

	public static Vector3 Transform(Vector3 position, Matrix4x4 matrix)
	{
		throw null;
	}

	public static Vector3 Transform(Vector3 value, Quaternion rotation)
	{
		throw null;
	}

	public static Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix)
	{
		throw null;
	}
}
