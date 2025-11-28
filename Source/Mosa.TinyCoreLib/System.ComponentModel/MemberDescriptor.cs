using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.ComponentModel;

public abstract class MemberDescriptor
{
	protected virtual Attribute[]? AttributeArray
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual AttributeCollection Attributes
	{
		get
		{
			throw null;
		}
	}

	public virtual string Category
	{
		get
		{
			throw null;
		}
	}

	public virtual string Description
	{
		get
		{
			throw null;
		}
	}

	public virtual bool DesignTimeOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual string DisplayName
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsBrowsable
	{
		get
		{
			throw null;
		}
	}

	public virtual string Name
	{
		get
		{
			throw null;
		}
	}

	protected virtual int NameHashCode
	{
		get
		{
			throw null;
		}
	}

	protected MemberDescriptor(MemberDescriptor descr)
	{
	}

	protected MemberDescriptor(MemberDescriptor oldMemberDescriptor, Attribute[]? newAttributes)
	{
	}

	protected MemberDescriptor(string name)
	{
	}

	protected MemberDescriptor(string name, Attribute[]? attributes)
	{
	}

	protected virtual AttributeCollection CreateAttributeCollection()
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	protected virtual void FillAttributes(IList attributeList)
	{
	}

	protected static MethodInfo? FindMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type componentClass, string name, Type[] args, Type returnType)
	{
		throw null;
	}

	protected static MethodInfo? FindMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type componentClass, string name, Type[] args, Type returnType, bool publicOnly)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected virtual object? GetInvocationTarget(Type type, object instance)
	{
		throw null;
	}

	[Obsolete("MemberDescriptor.GetInvokee has been deprecated. Use GetInvocationTarget instead.")]
	protected static object GetInvokee(Type componentClass, object component)
	{
		throw null;
	}

	protected static ISite? GetSite(object? component)
	{
		throw null;
	}
}
