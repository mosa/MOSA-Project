using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml.XPath;

public class XPathException : SystemException
{
	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public XPathException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected XPathException(SerializationInfo info, StreamingContext context)
	{
	}

	public XPathException(string? message)
	{
	}

	public XPathException(string? message, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
