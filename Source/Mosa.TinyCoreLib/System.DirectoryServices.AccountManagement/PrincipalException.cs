using System.Runtime.Serialization;

namespace System.DirectoryServices.AccountManagement;

public abstract class PrincipalException : SystemException
{
	private protected PrincipalException()
	{
	}

	protected PrincipalException(SerializationInfo info, StreamingContext context)
	{
	}
}
