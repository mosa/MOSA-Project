using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class ArrayTypeMismatchException : SystemException
{
	public ArrayTypeMismatchException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArrayTypeMismatchException(SerializationInfo info, StreamingContext context)
	{
	}

	public ArrayTypeMismatchException(string? message)
	{
	}

	public ArrayTypeMismatchException(string? message, Exception? innerException)
	{
	}
}
