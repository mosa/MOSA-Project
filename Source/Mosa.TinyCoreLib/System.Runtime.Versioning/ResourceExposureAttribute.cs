using System.Diagnostics;

namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
[Conditional("RESOURCE_ANNOTATION_WORK")]
public sealed class ResourceExposureAttribute : Attribute
{
	public ResourceScope ResourceExposureLevel
	{
		get
		{
			throw null;
		}
	}

	public ResourceExposureAttribute(ResourceScope exposureLevel)
	{
	}
}
