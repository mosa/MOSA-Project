using System.Collections.Generic;

namespace System.Reflection;

public abstract class MemberInfo : ICustomAttributeProvider
{
	public virtual IEnumerable<CustomAttributeData> CustomAttributes
	{
		get
		{
			throw null;
		}
	}

	public abstract Type? DeclaringType { get; }

	public virtual bool IsCollectible
	{
		get
		{
			throw null;
		}
	}

	public abstract MemberTypes MemberType { get; }

	public virtual int MetadataToken
	{
		get
		{
			throw null;
		}
	}

	public virtual Module Module
	{
		get
		{
			throw null;
		}
	}

	public abstract string Name { get; }

	public abstract Type? ReflectedType { get; }

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public abstract object[] GetCustomAttributes(bool inherit);

	public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

	public virtual IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public virtual bool HasSameMetadataDefinitionAs(MemberInfo other)
	{
		throw null;
	}

	public abstract bool IsDefined(Type attributeType, bool inherit);

	public static bool operator ==(MemberInfo? left, MemberInfo? right)
	{
		throw null;
	}

	public static bool operator !=(MemberInfo? left, MemberInfo? right)
	{
		throw null;
	}
}
