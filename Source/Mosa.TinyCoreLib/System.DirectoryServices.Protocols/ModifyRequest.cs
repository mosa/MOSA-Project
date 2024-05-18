namespace System.DirectoryServices.Protocols;

public class ModifyRequest : DirectoryRequest
{
	public string DistinguishedName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryAttributeModificationCollection Modifications
	{
		get
		{
			throw null;
		}
	}

	public ModifyRequest()
	{
	}

	public ModifyRequest(string distinguishedName, params DirectoryAttributeModification[] modifications)
	{
	}

	public ModifyRequest(string distinguishedName, DirectoryAttributeOperation operation, string attributeName, params object[] values)
	{
	}
}
