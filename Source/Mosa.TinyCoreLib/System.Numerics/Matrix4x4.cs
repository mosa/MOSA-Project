using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

public struct Matrix4x4 : IEquatable<Matrix4x4>
{
	public float M11;

	public float M12;

	public float M13;

	public float M14;

	public float M21;

	public float M22;

	public float M23;

	public float M24;

	public float M31;

	public float M32;

	public float M33;

	public float M34;

	public float M41;

	public float M42;

	public float M43;

	public float M44;

	public static Matrix4x4 Identity
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

	public Vector3 Translation
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public Matrix4x4(Matrix3x2 value)
	{
		throw null;
	}

	public Matrix4x4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
	{
		throw null;
	}

	public static Matrix4x4 Add(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public static Matrix4x4 CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3 cameraForwardVector)
	{
		throw null;
	}

	public static Matrix4x4 CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3 cameraForwardVector, Vector3 objectForwardVector)
	{
		throw null;
	}

	public static Matrix4x4 CreateFromAxisAngle(Vector3 axis, float angle)
	{
		throw null;
	}

	public static Matrix4x4 CreateFromQuaternion(Quaternion quaternion)
	{
		throw null;
	}

	public static Matrix4x4 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
	{
		throw null;
	}

	public static Matrix4x4 CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
	{
		throw null;
	}

	public static Matrix4x4 CreateLookAtLeftHanded(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
	{
		throw null;
	}

	public static Matrix4x4 CreateLookTo(Vector3 cameraPosition, Vector3 cameraDirection, Vector3 cameraUpVector)
	{
		throw null;
	}

	public static Matrix4x4 CreateLookToLeftHanded(Vector3 cameraPosition, Vector3 cameraDirection, Vector3 cameraUpVector)
	{
		throw null;
	}

	public static Matrix4x4 CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
	{
		throw null;
	}

	public static Matrix4x4 CreateOrthographicLeftHanded(float width, float height, float zNearPlane, float zFarPlane)
	{
		throw null;
	}

	public static Matrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
	{
		throw null;
	}

	public static Matrix4x4 CreateOrthographicOffCenterLeftHanded(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
	{
		throw null;
	}

	public static Matrix4x4 CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
	{
		throw null;
	}

	public static Matrix4x4 CreatePerspectiveLeftHanded(float width, float height, float nearPlaneDistance, float farPlaneDistance)
	{
		throw null;
	}

	public static Matrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
	{
		throw null;
	}

	public static Matrix4x4 CreatePerspectiveFieldOfViewLeftHanded(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
	{
		throw null;
	}

	public static Matrix4x4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
	{
		throw null;
	}

	public static Matrix4x4 CreatePerspectiveOffCenterLeftHanded(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
	{
		throw null;
	}

	public static Matrix4x4 CreateReflection(Plane value)
	{
		throw null;
	}

	public static Matrix4x4 CreateRotationX(float radians)
	{
		throw null;
	}

	public static Matrix4x4 CreateRotationX(float radians, Vector3 centerPoint)
	{
		throw null;
	}

	public static Matrix4x4 CreateRotationY(float radians)
	{
		throw null;
	}

	public static Matrix4x4 CreateRotationY(float radians, Vector3 centerPoint)
	{
		throw null;
	}

	public static Matrix4x4 CreateRotationZ(float radians)
	{
		throw null;
	}

	public static Matrix4x4 CreateRotationZ(float radians, Vector3 centerPoint)
	{
		throw null;
	}

	public static Matrix4x4 CreateScale(Vector3 scales)
	{
		throw null;
	}

	public static Matrix4x4 CreateScale(Vector3 scales, Vector3 centerPoint)
	{
		throw null;
	}

	public static Matrix4x4 CreateScale(float scale)
	{
		throw null;
	}

	public static Matrix4x4 CreateScale(float scale, Vector3 centerPoint)
	{
		throw null;
	}

	public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale)
	{
		throw null;
	}

	public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale, Vector3 centerPoint)
	{
		throw null;
	}

	public static Matrix4x4 CreateShadow(Vector3 lightDirection, Plane plane)
	{
		throw null;
	}

	public static Matrix4x4 CreateTranslation(Vector3 position)
	{
		throw null;
	}

	public static Matrix4x4 CreateTranslation(float xPosition, float yPosition, float zPosition)
	{
		throw null;
	}

	public static Matrix4x4 CreateViewport(float x, float y, float width, float height, float minDepth, float maxDepth)
	{
		throw null;
	}

	public static Matrix4x4 CreateViewportLeftHanded(float x, float y, float width, float height, float minDepth, float maxDepth)
	{
		throw null;
	}

	public static Matrix4x4 CreateWorld(Vector3 position, Vector3 forward, Vector3 up)
	{
		throw null;
	}

	public static bool Decompose(Matrix4x4 matrix, out Vector3 scale, out Quaternion rotation, out Vector3 translation)
	{
		throw null;
	}

	public readonly bool Equals(Matrix4x4 other)
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

	public static bool Invert(Matrix4x4 matrix, out Matrix4x4 result)
	{
		throw null;
	}

	public static Matrix4x4 Lerp(Matrix4x4 matrix1, Matrix4x4 matrix2, float amount)
	{
		throw null;
	}

	public static Matrix4x4 Multiply(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public static Matrix4x4 Multiply(Matrix4x4 value1, float value2)
	{
		throw null;
	}

	public static Matrix4x4 Negate(Matrix4x4 value)
	{
		throw null;
	}

	public static Matrix4x4 operator +(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public static bool operator ==(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public static bool operator !=(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public static Matrix4x4 operator *(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public static Matrix4x4 operator *(Matrix4x4 value1, float value2)
	{
		throw null;
	}

	public static Matrix4x4 operator -(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public static Matrix4x4 operator -(Matrix4x4 value)
	{
		throw null;
	}

	public static Matrix4x4 Subtract(Matrix4x4 value1, Matrix4x4 value2)
	{
		throw null;
	}

	public override readonly string ToString()
	{
		throw null;
	}

	public static Matrix4x4 Transform(Matrix4x4 value, Quaternion rotation)
	{
		throw null;
	}

	public static Matrix4x4 Transpose(Matrix4x4 matrix)
	{
		throw null;
	}
}
