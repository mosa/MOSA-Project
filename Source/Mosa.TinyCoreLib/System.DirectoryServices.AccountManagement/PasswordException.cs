using System.Runtime.Serialization;

namespace System.DirectoryServices.AccountManagement;

public class PasswordException : PrincipalException
{
	public PasswordException()
	{
	}

	protected PasswordException(SerializationInfo info, StreamingContext context)
	{
	}

	public PasswordException(string message)
	{
	}

	public PasswordException(string message, Exception innerException)
	{
	}
}
