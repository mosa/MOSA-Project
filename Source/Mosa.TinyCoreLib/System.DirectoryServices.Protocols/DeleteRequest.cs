namespace System.DirectoryServices.Protocols;

public class DeleteRequest : DirectoryRequest
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

	public DeleteRequest()
	{
	}

	public DeleteRequest(string distinguishedName)
	{
	}
}
