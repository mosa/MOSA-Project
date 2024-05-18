using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System;

[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
public abstract class Attribute
{
	public virtual object TypeId
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(Assembly element, Type attributeType)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(Assembly element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(MemberInfo element, Type attributeType)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(MemberInfo element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(Module element, Type attributeType)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(Module element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(ParameterInfo element, Type attributeType)
	{
		throw null;
	}

	public static Attribute? GetCustomAttribute(ParameterInfo element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Assembly element)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Assembly element, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(MemberInfo element)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(MemberInfo element, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(MemberInfo element, Type attributeType)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(MemberInfo element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Module element)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Module element, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Module element, Type attributeType)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(Module element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(ParameterInfo element)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(ParameterInfo element, bool inherit)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType)
	{
		throw null;
	}

	public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public virtual bool IsDefaultAttribute()
	{
		throw null;
	}

	public static bool IsDefined(Assembly element, Type attributeType)
	{
		throw null;
	}

	public static bool IsDefined(Assembly element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static bool IsDefined(MemberInfo element, Type attributeType)
	{
		throw null;
	}

	public static bool IsDefined(MemberInfo element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static bool IsDefined(Module element, Type attributeType)
	{
		throw null;
	}

	public static bool IsDefined(Module element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public static bool IsDefined(ParameterInfo element, Type attributeType)
	{
		throw null;
	}

	public static bool IsDefined(ParameterInfo element, Type attributeType, bool inherit)
	{
		throw null;
	}

	public virtual bool Match(object? obj)
	{
		throw null;
	}
}
