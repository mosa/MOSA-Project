using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ComEventInterfaceAttribute : Attribute
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)]
	public Type EventProvider
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public Type SourceInterface
	{
		get
		{
			throw null;
		}
	}

	public ComEventInterfaceAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type SourceInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)] Type EventProvider)
	{
	}
}
