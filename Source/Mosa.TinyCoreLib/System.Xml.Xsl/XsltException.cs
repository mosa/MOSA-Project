using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml.Xsl;

public class XsltException : SystemException
{
	public virtual int LineNumber
	{
		get
		{
			throw null;
		}
	}

	public virtual int LinePosition
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

	public virtual string? SourceUri
	{
		get
		{
			throw null;
		}
	}

	public XsltException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected XsltException(SerializationInfo info, StreamingContext context)
	{
	}

	public XsltException(string message)
	{
	}

	public XsltException(string message, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
