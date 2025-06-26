using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using Internal;

namespace System;

public class Exception : ISerializable
{
	public virtual IDictionary Data { get; }

	public virtual string? HelpLink { get; set; }

	public int HResult { get; set; }

	public Exception? InnerException { get; }

	public virtual string Message { get; }

	public virtual string? Source { get; set; }

	public virtual string? StackTrace { get; }

	public MethodBase? TargetSite
	{
		[RequiresUnreferencedCode("Metadata for the method might be incomplete or removed")]
		get;
	}

	[Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected event EventHandler<SafeSerializationEventArgs>? SerializeObjectState
	{
		add => throw new NotImplementedException();
		remove => throw new NotImplementedException();
	}

	public Exception() => Message = Exceptions.Exception;

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Exception(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();

	public Exception(string? message)
	{
		Message = message;
	}

	public Exception(string? message, Exception? innerException)
	{
		Message = message;
		InnerException = innerException;
	}

	public virtual Exception GetBaseException() => throw new NotImplementedException();

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		=> throw new NotImplementedException();

	public new Type GetType() => throw new NotImplementedException();

	public override string ToString() => throw new NotImplementedException();
}
