using System.Security.Permissions;

namespace System.Security.Policy;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class FileCodeGroup : CodeGroup
{
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

	public FileCodeGroup(IMembershipCondition membershipCondition, FileIOPermissionAccess access)
		: base(null, null)
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

	public override int GetHashCode()
	{
		throw null;
	}

	protected override void ParseXml(SecurityElement e, PolicyLevel level)
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
