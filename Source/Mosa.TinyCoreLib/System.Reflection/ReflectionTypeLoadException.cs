using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Reflection;

public sealed class ReflectionTypeLoadException : SystemException
{
	public Exception?[] LoaderExceptions
	{
		get
		{
			throw null;
		}
	}

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public Type?[] Types
	{
		get
		{
			throw null;
		}
	}

	public ReflectionTypeLoadException(Type?[]? classes, Exception?[]? exceptions)
	{
	}

	public ReflectionTypeLoadException(Type?[]? classes, Exception?[]? exceptions, string? message)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
