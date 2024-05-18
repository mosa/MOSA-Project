using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml.Xsl;

public class XsltCompileException : XsltException
{
	public XsltCompileException()
	{
	}

	public XsltCompileException(Exception inner, string sourceUri, int lineNumber, int linePosition)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected XsltCompileException(SerializationInfo info, StreamingContext context)
	{
	}

	public XsltCompileException(string message)
	{
	}

	public XsltCompileException(string message, Exception innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
