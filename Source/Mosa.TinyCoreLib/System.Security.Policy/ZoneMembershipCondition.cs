namespace System.Security.Policy;

public sealed class ZoneMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
{
	public SecurityZone SecurityZone
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ZoneMembershipCondition(SecurityZone zone)
	{
	}

	public bool Check(Evidence evidence)
	{
		throw null;
	}

	public IMembershipCondition Copy()
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public void FromXml(SecurityElement e)
	{
	}

	public void FromXml(SecurityElement e, PolicyLevel level)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public SecurityElement ToXml()
	{
		throw null;
	}

	public SecurityElement ToXml(PolicyLevel level)
	{
		throw null;
	}
}
