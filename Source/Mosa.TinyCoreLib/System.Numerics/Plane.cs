using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

public struct Plane : IEquatable<Plane>
{
	public Vector3 Normal;

	public float D;

	public Plane(Vector3 normal, float d)
	{
		throw null;
	}

	public Plane(Vector4 value)
	{
		throw null;
	}

	public Plane(float x, float y, float z, float d)
	{
		throw null;
	}

	public static Plane CreateFromVertices(Vector3 point1, Vector3 point2, Vector3 point3)
	{
		throw null;
	}

	public static float Dot(Plane plane, Vector4 value)
	{
		throw null;
	}

	public static float DotCoordinate(Plane plane, Vector3 value)
	{
		throw null;
	}

	public static float DotNormal(Plane plane, Vector3 value)
	{
		throw null;
	}

	public readonly bool Equals(Plane other)
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

	public static Plane Normalize(Plane value)
	{
		throw null;
	}

	public static bool operator ==(Plane value1, Plane value2)
	{
		throw null;
	}

	public static bool operator !=(Plane value1, Plane value2)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public static Plane Transform(Plane plane, Matrix4x4 matrix)
	{
		throw null;
	}

	public static Plane Transform(Plane plane, Quaternion rotation)
	{
		throw null;
	}
}
