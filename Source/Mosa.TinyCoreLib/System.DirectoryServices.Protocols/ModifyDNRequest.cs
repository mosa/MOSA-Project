namespace System.DirectoryServices.Protocols;

public class ModifyDNRequest : DirectoryRequest
{
	public bool DeleteOldRdn
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public string NewName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string NewParentDistinguishedName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ModifyDNRequest()
	{
	}

	public ModifyDNRequest(string distinguishedName, string newParentDistinguishedName, string newName)
	{
	}
}
