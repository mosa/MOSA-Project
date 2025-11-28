using System.Runtime.Serialization;

namespace System.DirectoryServices.AccountManagement;

public class NoMatchingPrincipalException : PrincipalException
{
	public NoMatchingPrincipalException()
	{
	}

	protected NoMatchingPrincipalException(SerializationInfo info, StreamingContext context)
	{
	}

	public NoMatchingPrincipalException(string message)
	{
	}

	public NoMatchingPrincipalException(string message, Exception innerException)
	{
	}
}
