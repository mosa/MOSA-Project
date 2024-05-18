using System.Runtime.Serialization;

namespace System.DirectoryServices.AccountManagement;

public class MultipleMatchesException : PrincipalException
{
	public MultipleMatchesException()
	{
	}

	protected MultipleMatchesException(SerializationInfo info, StreamingContext context)
	{
	}

	public MultipleMatchesException(string message)
	{
	}

	public MultipleMatchesException(string message, Exception innerException)
	{
	}
}
