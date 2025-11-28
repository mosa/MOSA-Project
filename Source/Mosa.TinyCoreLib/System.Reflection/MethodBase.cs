using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Reflection;

public abstract class MethodBase : MemberInfo
{
	public abstract MethodAttributes Attributes { get; }

	public virtual CallingConventions CallingConvention
	{
		get
		{
			throw null;
		}
	}

	public virtual bool ContainsGenericParameters
	{
		get
		{
			throw null;
		}
	}

	public bool IsAbstract => (Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract;

	public bool IsAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;

	public virtual bool IsConstructedGenericMethod
	{
		get
		{
			throw null;
		}
	}

	public bool IsConstructor => this is ConstructorInfo && !IsStatic && (Attributes & MethodAttributes.RTSpecialName)
		== MethodAttributes.RTSpecialName;

	public bool IsFamily => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;

	public bool IsFamilyAndAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;

	public bool IsFamilyOrAssembly => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;

	public bool IsFinal => (Attributes & MethodAttributes.Final) == MethodAttributes.Final;

	public virtual bool IsGenericMethod
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsGenericMethodDefinition
	{
		get
		{
			throw null;
		}
	}

	public bool IsHideBySig => (Attributes & MethodAttributes.HideBySig) == MethodAttributes.HideBySig;

	public bool IsPrivate => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;

	public bool IsPublic => (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;

	public virtual bool IsSecurityCritical
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSecuritySafeCritical
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSecurityTransparent
	{
		get
		{
			throw null;
		}
	}

	public bool IsSpecialName => (Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName;

	public bool IsStatic => (Attributes & MethodAttributes.Static) == MethodAttributes.Static;

	public bool IsVirtual => (Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual;

	public abstract RuntimeMethodHandle MethodHandle { get; }

	public virtual MethodImplAttributes MethodImplementationFlags
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Metadata for the method might be incomplete or removed")]
	public static MethodBase? GetCurrentMethod()
	{
		throw null;
	}

	public virtual Type[] GetGenericArguments()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming may change method bodies. For example it can change some instructions, remove branches or local variables.")]
	public virtual MethodBody? GetMethodBody()
	{
		throw null;
	}

	public static MethodBase? GetMethodFromHandle(RuntimeMethodHandle handle)
	{
		throw null;
	}

	public static MethodBase? GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType)
	{
		throw null;
	}

	public abstract MethodImplAttributes GetMethodImplementationFlags();

	public abstract ParameterInfo[] GetParameters();

	public object? Invoke(object? obj, object?[]? parameters)
	{
		throw null;
	}

	public abstract object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture);

	public static bool operator ==(MethodBase? left, MethodBase? right)
	{
		throw null;
	}

	public static bool operator !=(MethodBase? left, MethodBase? right)
	{
		throw null;
	}
}
