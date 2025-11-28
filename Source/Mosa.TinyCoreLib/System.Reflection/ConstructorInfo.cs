using System.Globalization;

namespace System.Reflection;

public abstract class ConstructorInfo : MethodBase
{
	public static readonly string ConstructorName = Internal.Impl.ConstructorInfo.ConstructorName;

	public static readonly string TypeConstructorName = Internal.Impl.ConstructorInfo.TypeConstructorName;

	public override MemberTypes MemberType
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

	public override int GetHashCode()
	{
		throw null;
	}

	public object Invoke(object?[]? parameters)
	{
		throw null;
	}

	public abstract object Invoke(BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture);

	public static bool operator ==(ConstructorInfo? left, ConstructorInfo? right)
	{
		throw null;
	}

	public static bool operator !=(ConstructorInfo? left, ConstructorInfo? right)
	{
		throw null;
	}
}
