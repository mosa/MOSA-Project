using System.Globalization;

namespace System.Reflection;

public abstract class PropertyInfo : MemberInfo
{
	public abstract PropertyAttributes Attributes { get; }

	public abstract bool CanRead { get; }

	public abstract bool CanWrite { get; }

	public virtual MethodInfo? GetMethod
	{
		get
		{
			throw null;
		}
	}

	public bool IsSpecialName => (Attributes & PropertyAttributes.SpecialName) == PropertyAttributes.SpecialName;

	public override MemberTypes MemberType
	{
		get
		{
			throw null;
		}
	}

	public abstract Type PropertyType { get; }

	public virtual MethodInfo? SetMethod
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

	public MethodInfo[] GetAccessors()
	{
		throw null;
	}

	public abstract MethodInfo[] GetAccessors(bool nonPublic);

	public virtual object? GetConstantValue()
	{
		throw null;
	}

	public MethodInfo? GetGetMethod()
	{
		throw null;
	}

	public abstract MethodInfo? GetGetMethod(bool nonPublic);

	public override int GetHashCode()
	{
		throw null;
	}

	public abstract ParameterInfo[] GetIndexParameters();

	public virtual Type[] GetOptionalCustomModifiers()
	{
		throw null;
	}

	public virtual object? GetRawConstantValue()
	{
		throw null;
	}

	public virtual Type GetModifiedPropertyType()
	{
		throw null;
	}

	public virtual Type[] GetRequiredCustomModifiers()
	{
		throw null;
	}

	public MethodInfo? GetSetMethod()
	{
		throw null;
	}

	public abstract MethodInfo? GetSetMethod(bool nonPublic);

	public object? GetValue(object? obj)
	{
		throw null;
	}

	public virtual object? GetValue(object? obj, object?[]? index)
	{
		throw null;
	}

	public abstract object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture);

	public static bool operator ==(PropertyInfo? left, PropertyInfo? right)
	{
		throw null;
	}

	public static bool operator !=(PropertyInfo? left, PropertyInfo? right)
	{
		throw null;
	}

	public void SetValue(object? obj, object? value)
	{
	}

	public virtual void SetValue(object? obj, object? value, object?[]? index)
	{
	}

	public abstract void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture);
}
