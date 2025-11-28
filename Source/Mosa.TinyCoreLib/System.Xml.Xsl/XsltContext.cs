using System.Xml.XPath;

namespace System.Xml.Xsl;

public abstract class XsltContext : XmlNamespaceManager
{
	public abstract bool Whitespace { get; }

	protected XsltContext()
		: base(null)
	{
	}

	protected XsltContext(NameTable table)
		: base(null)
	{
	}

	public abstract int CompareDocument(string baseUri, string nextbaseUri);

	public abstract bool PreserveWhitespace(XPathNavigator node);

	public abstract IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes);

	public abstract IXsltContextVariable ResolveVariable(string prefix, string name);
}
