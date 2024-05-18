using System.Runtime.Serialization;

namespace System.DirectoryServices.AccountManagement;

public class PrincipalExistsException : PrincipalException
{
	public PrincipalExistsException()
	{
	}

	protected PrincipalExistsException(SerializationInfo info, StreamingContext context)
	{
	}

	public PrincipalExistsException(string message)
	{
	}

	public PrincipalExistsException(string message, Exception innerException)
	{
	}
}
