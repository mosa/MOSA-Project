namespace System.Security.Policy;

public sealed class UrlMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
{
	public string Url
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public UrlMembershipCondition(string url)
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

	public override bool Equals(object obj)
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
