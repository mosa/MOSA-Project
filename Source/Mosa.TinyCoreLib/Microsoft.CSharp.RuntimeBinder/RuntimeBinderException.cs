using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CSharp.RuntimeBinder;

public class RuntimeBinderException : Exception
{
	public RuntimeBinderException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected RuntimeBinderException(SerializationInfo info, StreamingContext context)
	{
	}

	public RuntimeBinderException(string? message)
	{
	}

	public RuntimeBinderException(string? message, Exception? innerException)
	{
	}
}
