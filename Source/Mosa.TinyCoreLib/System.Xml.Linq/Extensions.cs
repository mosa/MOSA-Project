using System.Collections.Generic;

namespace System.Xml.Linq;

public static class Extensions
{
	public static IEnumerable<XElement> AncestorsAndSelf(this IEnumerable<XElement?> source)
	{
		throw null;
	}

	public static IEnumerable<XElement> AncestorsAndSelf(this IEnumerable<XElement?> source, XName? name)
	{
		throw null;
	}

	public static IEnumerable<XElement> Ancestors<T>(this IEnumerable<T?> source) where T : XNode
	{
		throw null;
	}

	public static IEnumerable<XElement> Ancestors<T>(this IEnumerable<T?> source, XName? name) where T : XNode
	{
		throw null;
	}

	public static IEnumerable<XAttribute> Attributes(this IEnumerable<XElement?> source)
	{
		throw null;
	}

	public static IEnumerable<XAttribute> Attributes(this IEnumerable<XElement?> source, XName? name)
	{
		throw null;
	}

	public static IEnumerable<XNode> DescendantNodesAndSelf(this IEnumerable<XElement?> source)
	{
		throw null;
	}

	public static IEnumerable<XNode> DescendantNodes<T>(this IEnumerable<T?> source) where T : XContainer
	{
		throw null;
	}

	public static IEnumerable<XElement> DescendantsAndSelf(this IEnumerable<XElement?> source)
	{
		throw null;
	}

	public static IEnumerable<XElement> DescendantsAndSelf(this IEnumerable<XElement?> source, XName? name)
	{
		throw null;
	}

	public static IEnumerable<XElement> Descendants<T>(this IEnumerable<T?> source) where T : XContainer
	{
		throw null;
	}

	public static IEnumerable<XElement> Descendants<T>(this IEnumerable<T?> source, XName? name) where T : XContainer
	{
		throw null;
	}

	public static IEnumerable<XElement> Elements<T>(this IEnumerable<T?> source) where T : XContainer
	{
		throw null;
	}

	public static IEnumerable<XElement> Elements<T>(this IEnumerable<T?> source, XName? name) where T : XContainer
	{
		throw null;
	}

	public static IEnumerable<T> InDocumentOrder<T>(this IEnumerable<T> source) where T : XNode?
	{
		throw null;
	}

	public static IEnumerable<XNode> Nodes<T>(this IEnumerable<T?> source) where T : XContainer
	{
		throw null;
	}

	public static void Remove(this IEnumerable<XAttribute?> source)
	{
	}

	public static void Remove<T>(this IEnumerable<T?> source) where T : XNode
	{
	}
}
