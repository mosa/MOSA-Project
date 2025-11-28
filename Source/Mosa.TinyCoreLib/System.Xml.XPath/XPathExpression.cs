using System.Collections;

namespace System.Xml.XPath;

public abstract class XPathExpression
{
	public abstract string Expression { get; }

	public abstract XPathResultType ReturnType { get; }

	internal XPathExpression()
	{
	}

	public abstract void AddSort(object expr, IComparer comparer);

	public abstract void AddSort(object expr, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType);

	public abstract XPathExpression Clone();

	public static XPathExpression Compile(string xpath)
	{
		throw null;
	}

	public static XPathExpression Compile(string xpath, IXmlNamespaceResolver? nsResolver)
	{
		throw null;
	}

	public abstract void SetContext(IXmlNamespaceResolver? nsResolver);

	public abstract void SetContext(XmlNamespaceManager nsManager);
}
