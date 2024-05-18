using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaValidationException : XmlSchemaException
{
	public object? SourceObject
	{
		get
		{
			throw null;
		}
	}

	public XmlSchemaValidationException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected XmlSchemaValidationException(SerializationInfo info, StreamingContext context)
	{
	}

	public XmlSchemaValidationException(string? message)
	{
	}

	public XmlSchemaValidationException(string? message, Exception? innerException)
	{
	}

	public XmlSchemaValidationException(string? message, Exception? innerException, int lineNumber, int linePosition)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	protected internal void SetSourceObject(object? sourceObject)
	{
	}
}
