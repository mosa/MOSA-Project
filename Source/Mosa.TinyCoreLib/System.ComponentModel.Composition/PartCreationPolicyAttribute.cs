namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PartCreationPolicyAttribute : Attribute
{
	public CreationPolicy CreationPolicy
	{
		get
		{
			throw null;
		}
	}

	public PartCreationPolicyAttribute(CreationPolicy creationPolicy)
	{
	}
}
