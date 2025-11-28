using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System;

public class ArgumentException : SystemException
{
	public override string Message => !string.IsNullOrEmpty(ParamName) ? $"{base.Message} ({ParamName})" : base.Message;

	public virtual string? ParamName { get; }

	public ArgumentException()
		: base(Internal.Exceptions.ArgumentException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArgumentException(SerializationInfo info, StreamingContext context) {}

	public ArgumentException(string? message)
		: base(message) {}

	public ArgumentException(string? message, Exception? innerException)
		: base(message, innerException) {}

	public ArgumentException(string? message, string? paramName)
		: base(message) => ParamName = paramName;

	public ArgumentException(string? message, string? paramName, Exception? innerException)
		: base(message, innerException) => ParamName = paramName;

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context) {}

	public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (string.IsNullOrEmpty(argument))
			throw new ArgumentException(Internal.Exceptions.ArgumentException, paramName);
	}

	public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
	{
		if (string.IsNullOrWhiteSpace(argument))
			throw new ArgumentException(Internal.Exceptions.ArgumentException, paramName);
	}
}
