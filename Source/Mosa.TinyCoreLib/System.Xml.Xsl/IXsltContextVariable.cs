using System.Xml.XPath;

namespace System.Xml.Xsl;

public interface IXsltContextVariable
{
	bool IsLocal { get; }

	bool IsParam { get; }

	XPathResultType VariableType { get; }

	object Evaluate(XsltContext xsltContext);
}
