using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Method)]
public sealed class DataObjectMethodAttribute : Attribute
{
	public bool IsDefault
	{
		get
		{
			throw null;
		}
	}

	public DataObjectMethodType MethodType
	{
		get
		{
			throw null;
		}
	}

	public DataObjectMethodAttribute(DataObjectMethodType methodType)
	{
	}

	public DataObjectMethodAttribute(DataObjectMethodType methodType, bool isDefault)
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

	public override bool Match([NotNullWhen(true)] object? obj)
	{
		throw null;
	}
}
