using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public class BarrierPostPhaseException : Exception
{
	public BarrierPostPhaseException()
	{
	}

	public BarrierPostPhaseException(Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected BarrierPostPhaseException(SerializationInfo info, StreamingContext context)
	{
	}

	public BarrierPostPhaseException(string? message)
	{
	}

	public BarrierPostPhaseException(string? message, Exception? innerException)
	{
	}
}
