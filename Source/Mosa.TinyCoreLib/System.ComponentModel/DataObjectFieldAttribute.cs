using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class DataObjectFieldAttribute : Attribute
{
	public bool IsIdentity
	{
		get
		{
			throw null;
		}
	}

	public bool IsNullable
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public bool PrimaryKey
	{
		get
		{
			throw null;
		}
	}

	public DataObjectFieldAttribute(bool primaryKey)
	{
	}

	public DataObjectFieldAttribute(bool primaryKey, bool isIdentity)
	{
	}

	public DataObjectFieldAttribute(bool primaryKey, bool isIdentity, bool isNullable)
	{
	}

	public DataObjectFieldAttribute(bool primaryKey, bool isIdentity, bool isNullable, int length)
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
}
