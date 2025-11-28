using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public abstract class MethodInfo : MethodBase
{
	public override MemberTypes MemberType
	{
		get
		{
			throw null;
		}
	}

	public virtual ParameterInfo ReturnParameter
	{
		get
		{
			throw null;
		}
	}

	public virtual Type ReturnType
	{
		get
		{
			throw null;
		}
	}

	public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

	public virtual Delegate CreateDelegate(Type delegateType)
	{
		throw null;
	}

	public virtual Delegate CreateDelegate(Type delegateType, object? target)
	{
		throw null;
	}

	public T CreateDelegate<T>() where T : Delegate
	{
		throw null;
	}

	public T CreateDelegate<T>(object? target) where T : Delegate
	{
		throw null;
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public abstract MethodInfo GetBaseDefinition();

	public override Type[] GetGenericArguments()
	{
		throw null;
	}

	public virtual MethodInfo GetGenericMethodDefinition()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
	[RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
	public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments)
	{
		throw null;
	}

	public static bool operator ==(MethodInfo? left, MethodInfo? right)
	{
		throw null;
	}

	public static bool operator !=(MethodInfo? left, MethodInfo? right)
	{
		throw null;
	}
}
