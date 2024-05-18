using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CSharp.RuntimeBinder;

public class RuntimeBinderInternalCompilerException : Exception
{
	public RuntimeBinderInternalCompilerException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected RuntimeBinderInternalCompilerException(SerializationInfo info, StreamingContext context)
	{
	}

	public RuntimeBinderInternalCompilerException(string? message)
	{
	}

	public RuntimeBinderInternalCompilerException(string? message, Exception? innerException)
	{
	}
}
