using System.Collections.Generic;

namespace System.Xml.Linq;

public abstract class XContainer : XNode
{
	public XNode? FirstNode
	{
		get
		{
			throw null;
		}
	}

	public XNode? LastNode
	{
		get
		{
			throw null;
		}
	}

	internal XContainer()
	{
	}

	public void Add(object? content)
	{
	}

	public void Add(params object?[] content)
	{
	}

	public void AddFirst(object? content)
	{
	}

	public void AddFirst(params object?[] content)
	{
	}

	public XmlWriter CreateWriter()
	{
		throw null;
	}

	public IEnumerable<XNode> DescendantNodes()
	{
		throw null;
	}

	public IEnumerable<XElement> Descendants()
	{
		throw null;
	}

	public IEnumerable<XElement> Descendants(XName? name)
	{
		throw null;
	}

	public XElement? Element(XName name)
	{
		throw null;
	}

	public IEnumerable<XElement> Elements()
	{
		throw null;
	}

	public IEnumerable<XElement> Elements(XName? name)
	{
		throw null;
	}

	public IEnumerable<XNode> Nodes()
	{
		throw null;
	}

	public void RemoveNodes()
	{
	}

	public void ReplaceNodes(object? content)
	{
	}

	public void ReplaceNodes(params object?[] content)
	{
	}
}
