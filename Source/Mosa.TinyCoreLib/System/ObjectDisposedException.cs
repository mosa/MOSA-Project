using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System;

public class ObjectDisposedException : InvalidOperationException
{
	public override string Message => !string.IsNullOrEmpty(ObjectName) ? $"{base.Message} ({ObjectName})" : base.Message;

	public string ObjectName { get; } = string.Empty;

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ObjectDisposedException(SerializationInfo info, StreamingContext context) {}

	public ObjectDisposedException(string? objectName)
		: base(Internal.Exceptions.ObjectDisposedException) => ObjectName = objectName ?? string.Empty;

	public ObjectDisposedException(string? message, Exception? innerException)
		: base(message, innerException) {}

	public ObjectDisposedException(string? objectName, string? message)
		: base(message) => ObjectName = objectName ?? string.Empty;

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context) {}

	public static void ThrowIf([DoesNotReturnIf(true)] bool condition, object instance)
	{
		if (condition)
			throw new ObjectDisposedException(instance.GetType().FullName);
	}

	public static void ThrowIf([DoesNotReturnIf(true)] bool condition, Type type)
	{
		if (condition)
			throw new ObjectDisposedException(type.FullName);
	}
}
