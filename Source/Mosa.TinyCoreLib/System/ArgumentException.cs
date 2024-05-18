using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System;

public class ArgumentException : SystemException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public virtual string? ParamName
	{
		get
		{
			throw null;
		}
	}

	public ArgumentException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArgumentException(SerializationInfo info, StreamingContext context)
	{
	}

	public ArgumentException(string? message)
	{
	}

	public ArgumentException(string? message, Exception? innerException)
	{
	}

	public ArgumentException(string? message, string? paramName)
	{
	}

	public ArgumentException(string? message, string? paramName, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		throw null;
	}

	public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		throw null;
	}
}
