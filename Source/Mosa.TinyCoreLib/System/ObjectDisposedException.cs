using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System;

public class ObjectDisposedException : InvalidOperationException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public string ObjectName
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ObjectDisposedException(SerializationInfo info, StreamingContext context)
	{
	}

	public ObjectDisposedException(string? objectName)
	{
	}

	public ObjectDisposedException(string? message, Exception? innerException)
	{
	}

	public ObjectDisposedException(string? objectName, string? message)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance)
	{
		throw null;
	}

	public static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type)
	{
		throw null;
	}
}
