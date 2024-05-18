using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols;

public class LdapException : DirectoryException, ISerializable
{
	public int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public PartialResultsCollection PartialResults
	{
		get
		{
			throw null;
		}
	}

	public string ServerErrorMessage
	{
		get
		{
			throw null;
		}
	}

	public LdapException()
	{
	}

	public LdapException(int errorCode)
	{
	}

	public LdapException(int errorCode, string message)
	{
	}

	public LdapException(int errorCode, string message, Exception inner)
	{
	}

	public LdapException(int errorCode, string message, string serverErrorMessage)
	{
	}

	protected LdapException(SerializationInfo info, StreamingContext context)
	{
	}

	public LdapException(string message)
	{
	}

	public LdapException(string message, Exception inner)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
