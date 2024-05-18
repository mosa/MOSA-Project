using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class MetadataTypeAttribute : Attribute
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public Type MetadataClassType
	{
		get
		{
			throw null;
		}
	}

	public MetadataTypeAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type metadataClassType)
	{
	}
}
