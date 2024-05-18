namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, Inherited = false)]
public sealed class DynamicallyAccessedMembersAttribute : Attribute
{
	public DynamicallyAccessedMemberTypes MemberTypes
	{
		get
		{
			throw null;
		}
	}

	public DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes memberTypes)
	{
	}
}
