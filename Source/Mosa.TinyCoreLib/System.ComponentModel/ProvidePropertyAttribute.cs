using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ProvidePropertyAttribute : Attribute
{
	public string PropertyName
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
	public string ReceiverTypeName
	{
		get
		{
			throw null;
		}
	}

	public override object TypeId
	{
		get
		{
			throw null;
		}
	}

	public ProvidePropertyAttribute(string propertyName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] string receiverTypeName)
	{
	}

	public ProvidePropertyAttribute(string propertyName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type receiverType)
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
