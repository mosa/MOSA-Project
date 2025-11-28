using System.Collections;

namespace System.Security.Policy;

public sealed class NetCodeGroup : CodeGroup
{
	public static readonly string AbsentOriginScheme;

	public static readonly string AnyOtherOriginScheme;

	public override string AttributeString
	{
		get
		{
			throw null;
		}
	}

	public override string MergeLogic
	{
		get
		{
			throw null;
		}
	}

	public override string PermissionSetName
	{
		get
		{
			throw null;
		}
	}

	public NetCodeGroup(IMembershipCondition membershipCondition)
		: base(null, null)
	{
	}

	public void AddConnectAccess(string originScheme, CodeConnectAccess connectAccess)
	{
	}

	public override CodeGroup Copy()
	{
		throw null;
	}

	protected override void CreateXml(SecurityElement element, PolicyLevel level)
	{
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public DictionaryEntry[] GetConnectAccessRules()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected override void ParseXml(SecurityElement e, PolicyLevel level)
	{
	}

	public void ResetConnectAccess()
	{
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
