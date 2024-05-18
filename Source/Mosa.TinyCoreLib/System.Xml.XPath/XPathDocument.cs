using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Xml.XPath;

public class XPathDocument : IXPathNavigable
{
	public XPathDocument(Stream stream)
	{
	}

	public XPathDocument(TextReader textReader)
	{
	}

	public XPathDocument([StringSyntax("Uri")] string uri)
	{
	}

	public XPathDocument([StringSyntax("Uri")] string uri, XmlSpace space)
	{
	}

	public XPathDocument(XmlReader reader)
	{
	}

	public XPathDocument(XmlReader reader, XmlSpace space)
	{
	}

	public XPathNavigator CreateNavigator()
	{
		throw null;
	}
}
