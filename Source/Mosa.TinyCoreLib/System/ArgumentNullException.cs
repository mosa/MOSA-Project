using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System;

public class ArgumentNullException : ArgumentException
{
	public ArgumentNullException()
		: base(Internal.Exceptions.ArgumentNullException) {}
	
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArgumentNullException(SerializationInfo info, StreamingContext context) {}

	public ArgumentNullException(string? paramName)
		: base(Internal.Exceptions.ArgumentNullException, paramName) {}

	public ArgumentNullException(string? message, Exception? innerException)
		: base(message, innerException) {}

	public ArgumentNullException(string? paramName, string? message)
		: base(message, paramName) {}

	public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (argument is null)
			throw new ArgumentNullException(paramName);
	}

	[CLSCompliant(false)]
	public static unsafe void ThrowIfNull([NotNull] void* argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (argument is null)
			throw new ArgumentNullException(paramName);
	}
}
