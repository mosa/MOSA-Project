using System.Collections;

namespace System.Security.Policy;

public abstract class CodeGroup
{
	public virtual string AttributeString
	{
		get
		{
			throw null;
		}
	}

	public IList Children
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IMembershipCondition MembershipCondition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract string MergeLogic { get; }

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string PermissionSetName
	{
		get
		{
			throw null;
		}
	}

	public PolicyStatement PolicyStatement
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected CodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy)
	{
	}

	public void AddChild(CodeGroup group)
	{
	}

	public abstract CodeGroup Copy();

	protected virtual void CreateXml(SecurityElement element, PolicyLevel level)
	{
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public bool Equals(CodeGroup cg, bool compareChildren)
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

	protected virtual void ParseXml(SecurityElement e, PolicyLevel level)
	{
	}

	public void RemoveChild(CodeGroup group)
	{
	}

	public abstract PolicyStatement Resolve(Evidence evidence);

	public abstract CodeGroup ResolveMatchingCodeGroups(Evidence evidence);

	public SecurityElement ToXml()
	{
		throw null;
	}

	public SecurityElement ToXml(PolicyLevel level)
	{
		throw null;
	}
}
