using System.Globalization;

namespace System.Reflection;

public abstract class FieldInfo : MemberInfo
{
	public abstract FieldAttributes Attributes { get; }

	public abstract RuntimeFieldHandle FieldHandle { get; }

	public abstract Type FieldType { get; }

	public bool IsAssembly
	{
		get
		{
			throw null;
		}
	}

	public bool IsFamily
	{
		get
		{
			throw null;
		}
	}

	public bool IsFamilyAndAssembly
	{
		get
		{
			throw null;
		}
	}

	public bool IsFamilyOrAssembly
	{
		get
		{
			throw null;
		}
	}

	public bool IsInitOnly
	{
		get
		{
			throw null;
		}
	}

	public bool IsLiteral
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public bool IsNotSerialized
	{
		get
		{
			throw null;
		}
	}

	public bool IsPinvokeImpl
	{
		get
		{
			throw null;
		}
	}

	public bool IsPrivate
	{
		get
		{
			throw null;
		}
	}

	public bool IsPublic
	{
		get
		{
			throw null;
		}
	}

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

	public bool IsSpecialName
	{
		get
		{
			throw null;
		}
	}

	public bool IsStatic
	{
		get
		{
			throw null;
		}
	}

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

	public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
	{
		throw null;
	}

	public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public virtual Type GetModifiedFieldType()
	{
		throw null;
	}

	public virtual Type[] GetOptionalCustomModifiers()
	{
		throw null;
	}

	public virtual object? GetRawConstantValue()
	{
		throw null;
	}

	public virtual Type[] GetRequiredCustomModifiers()
	{
		throw null;
	}

	public abstract object? GetValue(object? obj);

	[CLSCompliant(false)]
	public virtual object? GetValueDirect(TypedReference obj)
	{
		throw null;
	}

	public static bool operator ==(FieldInfo? left, FieldInfo? right)
	{
		throw null;
	}

	public static bool operator !=(FieldInfo? left, FieldInfo? right)
	{
		throw null;
	}

	public void SetValue(object? obj, object? value)
	{
	}

	public abstract void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, CultureInfo? culture);

	[CLSCompliant(false)]
	public virtual void SetValueDirect(TypedReference obj, object value)
	{
	}
}
