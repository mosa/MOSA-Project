using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class MetadataUpdateHandlerAttribute : Attribute
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public Type HandlerType
	{
		get
		{
			throw null;
		}
	}

	public MetadataUpdateHandlerAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type handlerType)
	{
	}
}
