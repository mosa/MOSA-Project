using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading.Tasks;

public class TaskCanceledException : OperationCanceledException
{
	public Task? Task
	{
		get
		{
			throw null;
		}
	}

	public TaskCanceledException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TaskCanceledException(SerializationInfo info, StreamingContext context)
	{
	}

	public TaskCanceledException(string? message)
	{
	}

	public TaskCanceledException(string? message, Exception? innerException)
	{
	}

	public TaskCanceledException(string? message, Exception? innerException, CancellationToken token)
	{
	}

	public TaskCanceledException(Task? task)
	{
	}
}
