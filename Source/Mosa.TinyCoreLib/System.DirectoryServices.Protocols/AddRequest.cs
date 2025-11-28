namespace System.DirectoryServices.Protocols;

public class AddRequest : DirectoryRequest
{
	public DirectoryAttributeCollection Attributes
	{
		get
		{
			throw null;
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

	public AddRequest()
	{
	}

	public AddRequest(string distinguishedName, params DirectoryAttribute[] attributes)
	{
	}

	public AddRequest(string distinguishedName, string objectClass)
	{
	}
}
