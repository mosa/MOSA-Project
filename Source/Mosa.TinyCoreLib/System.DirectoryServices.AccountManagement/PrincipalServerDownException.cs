using System.Runtime.Serialization;

namespace System.DirectoryServices.AccountManagement;

public class PrincipalServerDownException : PrincipalException
{
	public PrincipalServerDownException()
	{
	}

	protected PrincipalServerDownException(SerializationInfo info, StreamingContext context)
		: base(null, default(StreamingContext))
	{
	}

	public PrincipalServerDownException(string message)
	{
	}

	public PrincipalServerDownException(string message, Exception innerException)
	{
	}

	public PrincipalServerDownException(string message, Exception innerException, int errorCode)
	{
	}

	public PrincipalServerDownException(string message, Exception innerException, int errorCode, string serverName)
	{
	}

	public PrincipalServerDownException(string message, int errorCode)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
