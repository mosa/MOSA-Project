using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Markup;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public sealed class ValueSerializerAttribute : Attribute
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public Type ValueSerializerType
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public string ValueSerializerTypeName
	{
		get
		{
			throw null;
		}
	}

	public ValueSerializerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] string valueSerializerTypeName)
	{
	}

	public ValueSerializerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type valueSerializerType)
	{
	}
}
