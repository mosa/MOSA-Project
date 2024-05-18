using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System;

public class ArgumentNullException : ArgumentException
{
	public ArgumentNullException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArgumentNullException(SerializationInfo info, StreamingContext context)
	{
	}

	public ArgumentNullException(string? paramName)
	{
	}

	public ArgumentNullException(string? message, Exception? innerException)
	{
	}

	public ArgumentNullException(string? paramName, string? message)
	{
	}

	public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void ThrowIfNull([NotNull] void* argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		throw null;
	}
}
