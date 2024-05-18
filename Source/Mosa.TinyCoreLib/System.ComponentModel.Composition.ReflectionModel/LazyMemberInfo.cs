using System.Reflection;

namespace System.ComponentModel.Composition.ReflectionModel;

public struct LazyMemberInfo : IEquatable<LazyMemberInfo>
{
	private object _dummy;

	private int _dummyPrimitive;

	public MemberTypes MemberType
	{
		get
		{
			throw null;
		}
	}

	public LazyMemberInfo(MemberInfo member)
	{
		throw null;
	}

	public LazyMemberInfo(MemberTypes memberType, Func<MemberInfo[]> accessorsCreator)
	{
		throw null;
	}

	public LazyMemberInfo(MemberTypes memberType, params MemberInfo[] accessors)
	{
		throw null;
	}

	public bool Equals(LazyMemberInfo other)
	{
		throw null;
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public MemberInfo[] GetAccessors()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(LazyMemberInfo left, LazyMemberInfo right)
	{
		throw null;
	}

	public static bool operator !=(LazyMemberInfo left, LazyMemberInfo right)
	{
		throw null;
	}
}
