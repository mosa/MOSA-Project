using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class TypeUnloadedException : SystemException
{
	public TypeUnloadedException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TypeUnloadedException(SerializationInfo info, StreamingContext context)
	{
	}

	public TypeUnloadedException(string? message)
	{
	}

	public TypeUnloadedException(string? message, Exception? innerException)
	{
	}
}
