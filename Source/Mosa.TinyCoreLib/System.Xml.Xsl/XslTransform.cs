using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.XPath;

namespace System.Xml.Xsl;

public sealed class XslTransform
{
	public XmlResolver? XmlResolver
	{
		set
		{
		}
	}

	public void Load([StringSyntax("Uri")] string url)
	{
	}

	public void Load([StringSyntax("Uri")] string url, XmlResolver? resolver)
	{
	}

	public void Load(XmlReader stylesheet)
	{
	}

	public void Load(XmlReader stylesheet, XmlResolver? resolver)
	{
	}

	public void Load(IXPathNavigable stylesheet)
	{
	}

	public void Load(IXPathNavigable stylesheet, XmlResolver? resolver)
	{
	}

	public void Load(XPathNavigator stylesheet)
	{
	}

	public void Load(XPathNavigator stylesheet, XmlResolver? resolver)
	{
	}

	public void Transform(string inputfile, string outputfile)
	{
	}

	public void Transform(string inputfile, string outputfile, XmlResolver? resolver)
	{
	}

	public XmlReader Transform(IXPathNavigable input, XsltArgumentList? args)
	{
		throw null;
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? args, Stream output)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? args, Stream output, XmlResolver? resolver)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? args, TextWriter output)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? args, TextWriter output, XmlResolver? resolver)
	{
	}

	public XmlReader Transform(IXPathNavigable input, XsltArgumentList? args, XmlResolver? resolver)
	{
		throw null;
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? args, XmlWriter output)
	{
	}

	public void Transform(IXPathNavigable input, XsltArgumentList? args, XmlWriter output, XmlResolver? resolver)
	{
	}

	public XmlReader Transform(XPathNavigator input, XsltArgumentList? args)
	{
		throw null;
	}

	public void Transform(XPathNavigator input, XsltArgumentList? args, Stream output)
	{
	}

	public void Transform(XPathNavigator input, XsltArgumentList? args, Stream output, XmlResolver? resolver)
	{
	}

	public void Transform(XPathNavigator input, XsltArgumentList? args, TextWriter output)
	{
	}

	public void Transform(XPathNavigator input, XsltArgumentList? args, TextWriter output, XmlResolver? resolver)
	{
	}

	public XmlReader Transform(XPathNavigator input, XsltArgumentList? args, XmlResolver? resolver)
	{
		throw null;
	}

	public void Transform(XPathNavigator input, XsltArgumentList? args, XmlWriter output)
	{
	}

	public void Transform(XPathNavigator input, XsltArgumentList? args, XmlWriter output, XmlResolver? resolver)
	{
	}
}
