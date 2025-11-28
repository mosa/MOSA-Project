namespace System;

public class Object
{
	public virtual bool Equals(object? obj) => Internal.Impl.RuntimeHelpers.Equals(this, obj);

	public static bool Equals(object? objA, object? objB) => ReferenceEquals(objA, objB) || objA.Equals(objB);

	~Object() {}

	public virtual int GetHashCode() => Internal.Impl.RuntimeHelpers.GetHashCode(this);

	public Type GetType() => Internal.Impl.Object.GetType(this);

	protected object MemberwiseClone() => Internal.Impl.Object.MemberwiseClone(this);

	public static bool ReferenceEquals(object? objA, object? objB)
	{
		if (objA is null || objB is null)
			return false;

		return objA == objB;
	}

	public virtual string? ToString() => GetType().ToString();
}
