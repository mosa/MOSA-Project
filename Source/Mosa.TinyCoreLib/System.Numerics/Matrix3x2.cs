using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

public struct Matrix3x2 : IEquatable<Matrix3x2>
{
	public float M11;

	public float M12;

	public float M21;

	public float M22;

	public float M31;

	public float M32;

	public static Matrix3x2 Identity
	{
		get
		{
			throw null;
		}
	}

	public float this[int row, int column]
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

	public readonly bool IsIdentity
	{
		get
		{
			throw null;
		}
	}

	public Vector2 Translation
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public Matrix3x2(float m11, float m12, float m21, float m22, float m31, float m32)
	{
		throw null;
	}

	public static Matrix3x2 Add(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public static Matrix3x2 CreateRotation(float radians)
	{
		throw null;
	}

	public static Matrix3x2 CreateRotation(float radians, Vector2 centerPoint)
	{
		throw null;
	}

	public static Matrix3x2 CreateScale(Vector2 scales)
	{
		throw null;
	}

	public static Matrix3x2 CreateScale(Vector2 scales, Vector2 centerPoint)
	{
		throw null;
	}

	public static Matrix3x2 CreateScale(float scale)
	{
		throw null;
	}

	public static Matrix3x2 CreateScale(float scale, Vector2 centerPoint)
	{
		throw null;
	}

	public static Matrix3x2 CreateScale(float xScale, float yScale)
	{
		throw null;
	}

	public static Matrix3x2 CreateScale(float xScale, float yScale, Vector2 centerPoint)
	{
		throw null;
	}

	public static Matrix3x2 CreateSkew(float radiansX, float radiansY)
	{
		throw null;
	}

	public static Matrix3x2 CreateSkew(float radiansX, float radiansY, Vector2 centerPoint)
	{
		throw null;
	}

	public static Matrix3x2 CreateTranslation(Vector2 position)
	{
		throw null;
	}

	public static Matrix3x2 CreateTranslation(float xPosition, float yPosition)
	{
		throw null;
	}

	public readonly bool Equals(Matrix3x2 other)
	{
		throw null;
	}

	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public readonly float GetDeterminant()
	{
		throw null;
	}

	public override readonly int GetHashCode()
	{
		throw null;
	}

	public static bool Invert(Matrix3x2 matrix, out Matrix3x2 result)
	{
		throw null;
	}

	public static Matrix3x2 Lerp(Matrix3x2 matrix1, Matrix3x2 matrix2, float amount)
	{
		throw null;
	}

	public static Matrix3x2 Multiply(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public static Matrix3x2 Multiply(Matrix3x2 value1, float value2)
	{
		throw null;
	}

	public static Matrix3x2 Negate(Matrix3x2 value)
	{
		throw null;
	}

	public static Matrix3x2 operator +(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public static bool operator ==(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public static bool operator !=(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public static Matrix3x2 operator *(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public static Matrix3x2 operator *(Matrix3x2 value1, float value2)
	{
		throw null;
	}

	public static Matrix3x2 operator -(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public static Matrix3x2 operator -(Matrix3x2 value)
	{
		throw null;
	}

	public static Matrix3x2 Subtract(Matrix3x2 value1, Matrix3x2 value2)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}
}
