using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading.Tasks;

public class TaskSchedulerException : Exception
{
	public TaskSchedulerException()
	{
	}

	public TaskSchedulerException(Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TaskSchedulerException(SerializationInfo info, StreamingContext context)
	{
	}

	public TaskSchedulerException(string? message)
	{
	}

	public TaskSchedulerException(string? message, Exception? innerException)
	{
	}
}
