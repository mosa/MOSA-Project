using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DataObjectAttribute : Attribute
{
	public static readonly DataObjectAttribute DataObject;

	public static readonly DataObjectAttribute Default;

	public static readonly DataObjectAttribute NonDataObject;

	public bool IsDataObject
	{
		get
		{
			throw null;
		}
	}

	public DataObjectAttribute()
	{
	}

	public DataObjectAttribute(bool isDataObject)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override bool IsDefaultAttribute()
	{
		throw null;
	}
}
