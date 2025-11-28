using System.Collections.Generic;
using System.Xml.Linq;

namespace System.Xml.XPath;

public static class Extensions
{
	public static XPathNavigator CreateNavigator(this XNode node)
	{
		throw null;
	}

	public static XPathNavigator CreateNavigator(this XNode node, XmlNameTable? nameTable)
	{
		throw null;
	}

	public static object XPathEvaluate(this XNode node, string expression)
	{
		throw null;
	}

	public static object XPathEvaluate(this XNode node, string expression, IXmlNamespaceResolver? resolver)
	{
		throw null;
	}

	public static XElement? XPathSelectElement(this XNode node, string expression)
	{
		throw null;
	}

	public static XElement? XPathSelectElement(this XNode node, string expression, IXmlNamespaceResolver? resolver)
	{
		throw null;
	}

	public static IEnumerable<XElement> XPathSelectElements(this XNode node, string expression)
	{
		throw null;
	}

	public static IEnumerable<XElement> XPathSelectElements(this XNode node, string expression, IXmlNamespaceResolver? resolver)
	{
		throw null;
	}
}
