using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public sealed class TypeInitializationException : SystemException
{
	public string TypeName
	{
		get
		{
			throw null;
		}
	}

	public TypeInitializationException(string? fullTypeName, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
