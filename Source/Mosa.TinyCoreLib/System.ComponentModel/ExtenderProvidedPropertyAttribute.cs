using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class ExtenderProvidedPropertyAttribute : Attribute
{
	public PropertyDescriptor? ExtenderProperty
	{
		get
		{
			throw null;
		}
	}

	public IExtenderProvider? Provider
	{
		get
		{
			throw null;
		}
	}

	public Type? ReceiverType
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

	public override int GetHashCode()
	{
		throw null;
	}

	public override bool IsDefaultAttribute()
	{
		throw null;
	}
}
