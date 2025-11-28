using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.IO.IsolatedStorage;

public class IsolatedStorageException : Exception
{
	public IsolatedStorageException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected IsolatedStorageException(SerializationInfo info, StreamingContext context)
	{
	}

	public IsolatedStorageException(string? message)
	{
	}

	public IsolatedStorageException(string? message, Exception? inner)
	{
	}
}
