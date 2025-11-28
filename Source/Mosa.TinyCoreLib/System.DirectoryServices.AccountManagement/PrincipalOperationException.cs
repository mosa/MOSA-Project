using System.Runtime.Serialization;

namespace System.DirectoryServices.AccountManagement;

public class PrincipalOperationException : PrincipalException
{
	public int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public PrincipalOperationException()
	{
	}

	protected PrincipalOperationException(SerializationInfo info, StreamingContext context)
	{
	}

	public PrincipalOperationException(string message)
	{
	}

	public PrincipalOperationException(string message, Exception innerException)
	{
	}

	public PrincipalOperationException(string message, Exception innerException, int errorCode)
	{
	}

	public PrincipalOperationException(string message, int errorCode)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
