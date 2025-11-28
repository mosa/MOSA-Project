using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaException : SystemException
{
	public int LineNumber
	{
		get
		{
			throw null;
		}
	}

	public int LinePosition
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

	public XmlSchemaObject? SourceSchemaObject
	{
		get
		{
			throw null;
		}
	}

	public string? SourceUri
	{
		get
		{
			throw null;
		}
	}

	public XmlSchemaException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected XmlSchemaException(SerializationInfo info, StreamingContext context)
	{
	}

	public XmlSchemaException(string? message)
	{
	}

	public XmlSchemaException(string? message, Exception? innerException)
	{
	}

	public XmlSchemaException(string? message, Exception? innerException, int lineNumber, int linePosition)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
