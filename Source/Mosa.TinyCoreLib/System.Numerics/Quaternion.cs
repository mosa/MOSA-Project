using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

public struct Quaternion : IEquatable<Quaternion>
{
	public float X;

	public float Y;

	public float Z;

	public float W;

	public static Quaternion Zero
	{
		get
		{
			throw null;
		}
	}

	public static Quaternion Identity
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

	public readonly bool IsIdentity
	{
		get
		{
			throw null;
		}
	}

	public Quaternion(Vector3 vectorPart, float scalarPart)
	{
		throw null;
	}

	public Quaternion(float x, float y, float z, float w)
	{
		throw null;
	}

	public static Quaternion Add(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static Quaternion Conjugate(Quaternion value)
	{
		throw null;
	}

	public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
	{
		throw null;
	}

	public static Quaternion CreateFromRotationMatrix(Matrix4x4 matrix)
	{
		throw null;
	}

	public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
	{
		throw null;
	}

	public static Quaternion Divide(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static float Dot(Quaternion quaternion1, Quaternion quaternion2)
	{
		throw null;
	}

	public readonly bool Equals(Quaternion other)
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

	public static Quaternion Inverse(Quaternion value)
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

	public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
	{
		throw null;
	}

	public static Quaternion Multiply(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static Quaternion Multiply(Quaternion value1, float value2)
	{
		throw null;
	}

	public static Quaternion Negate(Quaternion value)
	{
		throw null;
	}

	public static Quaternion Normalize(Quaternion value)
	{
		throw null;
	}

	public static Quaternion operator +(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static Quaternion operator /(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static bool operator ==(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static bool operator !=(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static Quaternion operator *(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static Quaternion operator *(Quaternion value1, float value2)
	{
		throw null;
	}

	public static Quaternion operator -(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public static Quaternion operator -(Quaternion value)
	{
		throw null;
	}

	public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
	{
		throw null;
	}

	public static Quaternion Subtract(Quaternion value1, Quaternion value2)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}
}
