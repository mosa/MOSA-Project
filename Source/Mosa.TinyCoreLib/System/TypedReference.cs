using System.Reflection;

namespace System;

[CLSCompliant(false)]
public ref struct TypedReference
{
	private object _dummy;

	private int _dummyPrimitive;

	public override bool Equals(object? o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static Type GetTargetType(TypedReference value)
	{
		throw null;
	}

	public static TypedReference MakeTypedReference(object target, FieldInfo[] flds)
	{
		throw null;
	}

	public static void SetTypedReference(TypedReference target, object? value)
	{
	}

	public static RuntimeTypeHandle TargetTypeToken(TypedReference value)
	{
		throw null;
	}

	public static object ToObject(TypedReference value)
	{
		throw null;
	}
}
