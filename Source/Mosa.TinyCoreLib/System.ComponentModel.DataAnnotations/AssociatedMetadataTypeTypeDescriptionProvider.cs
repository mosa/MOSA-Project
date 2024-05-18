using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

public class AssociatedMetadataTypeTypeDescriptionProvider : TypeDescriptionProvider
{
	public AssociatedMetadataTypeTypeDescriptionProvider(Type type)
	{
	}

	public AssociatedMetadataTypeTypeDescriptionProvider(Type type, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type associatedMetadataType)
	{
	}

	public override ICustomTypeDescriptor GetTypeDescriptor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type objectType, object? instance)
	{
		throw null;
	}
}
