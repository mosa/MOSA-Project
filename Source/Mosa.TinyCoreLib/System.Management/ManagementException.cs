using System.Runtime.Serialization;

namespace System.Management;

public class ManagementException : SystemException
{
	public ManagementStatus ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public ManagementBaseObject ErrorInformation
	{
		get
		{
			throw null;
		}
	}

	public ManagementException()
	{
	}

	protected ManagementException(SerializationInfo info, StreamingContext context)
	{
	}

	public ManagementException(string message)
	{
	}

	public ManagementException(string message, Exception innerException)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
