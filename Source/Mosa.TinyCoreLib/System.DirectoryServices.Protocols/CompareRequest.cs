namespace System.DirectoryServices.Protocols;

public class CompareRequest : DirectoryRequest
{
	public DirectoryAttribute Assertion
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

	public CompareRequest()
	{
	}

	public CompareRequest(string distinguishedName, DirectoryAttribute assertion)
	{
	}

	public CompareRequest(string distinguishedName, string attributeName, byte[] value)
	{
	}

	public CompareRequest(string distinguishedName, string attributeName, string value)
	{
	}

	public CompareRequest(string distinguishedName, string attributeName, Uri value)
	{
	}
}
