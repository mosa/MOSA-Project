using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation;

public class NetworkInformationException : Win32Exception
{
	public override int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public NetworkInformationException()
	{
	}

	public NetworkInformationException(int errorCode)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NetworkInformationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
