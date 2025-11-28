namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
public sealed class ComponentGuaranteesAttribute : Attribute
{
	public ComponentGuaranteesOptions Guarantees
	{
		get
		{
			throw null;
		}
	}

	public ComponentGuaranteesAttribute(ComponentGuaranteesOptions guarantees)
	{
	}
}
