using System.Globalization;

namespace System.Reflection;

public abstract class FieldInfo : MemberInfo
{
	public abstract FieldAttributes Attributes { get; }

	public abstract RuntimeFieldHandle FieldHandle { get; }

	public abstract Type FieldType { get; }

	public bool IsAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;

	public bool IsFamily => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;

	public bool IsFamilyAndAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;

	public bool IsFamilyOrAssembly => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;

	public bool IsInitOnly => (Attributes & FieldAttributes.InitOnly) == FieldAttributes.InitOnly;

	public bool IsLiteral => (Attributes & FieldAttributes.Literal) == FieldAttributes.Literal;

	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public bool IsNotSerialized => (Attributes & FieldAttributes.NotSerialized) == FieldAttributes.NotSerialized;

	public bool IsPinvokeImpl => (Attributes & FieldAttributes.PinvokeImpl) == FieldAttributes.PinvokeImpl;

	public bool IsPrivate => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;

	public bool IsPublic => (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;

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

	public bool IsSpecialName => (Attributes & FieldAttributes.SpecialName) == FieldAttributes.SpecialName;

	public bool IsStatic => (Attributes & FieldAttributes.Static) == FieldAttributes.Static;

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
