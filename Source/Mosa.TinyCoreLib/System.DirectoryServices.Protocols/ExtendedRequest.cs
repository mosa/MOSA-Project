namespace System.DirectoryServices.Protocols;

public class ExtendedRequest : DirectoryRequest
{
	public string RequestName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] RequestValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ExtendedRequest()
	{
	}

	public ExtendedRequest(string requestName)
	{
	}

	public ExtendedRequest(string requestName, byte[] requestValue)
	{
	}
}
