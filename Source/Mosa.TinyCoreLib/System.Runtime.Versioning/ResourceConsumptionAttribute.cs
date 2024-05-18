using System.Diagnostics;

namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[Conditional("RESOURCE_ANNOTATION_WORK")]
public sealed class ResourceConsumptionAttribute : Attribute
{
	public ResourceScope ConsumptionScope
	{
		get
		{
			throw null;
		}
	}

	public ResourceScope ResourceScope
	{
		get
		{
			throw null;
		}
	}

	public ResourceConsumptionAttribute(ResourceScope resourceScope)
	{
	}

	public ResourceConsumptionAttribute(ResourceScope resourceScope, ResourceScope consumptionScope)
	{
	}
}
