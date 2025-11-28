using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaInferenceException : XmlSchemaException
{
	public XmlSchemaInferenceException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected XmlSchemaInferenceException(SerializationInfo info, StreamingContext context)
	{
	}

	public XmlSchemaInferenceException(string message)
	{
	}

	public XmlSchemaInferenceException(string message, Exception? innerException)
	{
	}

	public XmlSchemaInferenceException(string message, Exception? innerException, int lineNumber, int linePosition)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
