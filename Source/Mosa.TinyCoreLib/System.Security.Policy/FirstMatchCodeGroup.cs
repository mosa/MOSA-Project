namespace System.Security.Policy;

[Obsolete("This type is obsolete. See https://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
public sealed class FirstMatchCodeGroup : CodeGroup
{
	public override string MergeLogic
	{
		get
		{
			throw null;
		}
	}

	public FirstMatchCodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy)
		: base(null, null)
	{
	}

	public override CodeGroup Copy()
	{
		throw null;
	}

	public override PolicyStatement Resolve(Evidence evidence)
	{
		throw null;
	}

	public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
	{
		throw null;
	}
}
