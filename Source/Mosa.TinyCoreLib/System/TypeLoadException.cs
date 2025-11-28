using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class TypeLoadException : SystemException
{
	public override string Message => !string.IsNullOrEmpty(TypeName) ? $"{base.Message} ({TypeName})" : base.Message;

	public string TypeName => throw new NotImplementedException();

	public TypeLoadException()
		: base(Internal.Exceptions.TypeLoadException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected TypeLoadException(SerializationInfo info, StreamingContext context) {}

	public TypeLoadException(string? message)
		: base(message) {}

	public TypeLoadException(string? message, Exception? inner)
		: base(message, inner) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context) {}
}
