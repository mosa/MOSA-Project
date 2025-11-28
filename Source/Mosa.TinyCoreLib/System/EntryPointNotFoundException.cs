using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class EntryPointNotFoundException : TypeLoadException
{
	public EntryPointNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected EntryPointNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public EntryPointNotFoundException(string? message)
	{
	}

	public EntryPointNotFoundException(string? message, Exception? inner)
	{
	}
}
