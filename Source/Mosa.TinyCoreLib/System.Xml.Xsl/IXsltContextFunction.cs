using System.Xml.XPath;

namespace System.Xml.Xsl;

public interface IXsltContextFunction
{
	XPathResultType[] ArgTypes { get; }

	int Maxargs { get; }

	int Minargs { get; }

	XPathResultType ReturnType { get; }

	object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext);
}
