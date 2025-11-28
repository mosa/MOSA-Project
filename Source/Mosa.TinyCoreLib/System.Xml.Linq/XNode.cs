using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

public abstract class XNode : XObject
{
	public static XNodeDocumentOrderComparer DocumentOrderComparer
	{
		get
		{
			throw null;
		}
	}

	public static XNodeEqualityComparer EqualityComparer
	{
		get
		{
			throw null;
		}
	}

	public XNode? NextNode
	{
		get
		{
			throw null;
		}
	}

	public XNode? PreviousNode
	{
		get
		{
			throw null;
		}
	}

	internal XNode()
	{
	}

	public void AddAfterSelf(object? content)
	{
	}

	public void AddAfterSelf(params object?[] content)
	{
	}

	public void AddBeforeSelf(object? content)
	{
	}

	public void AddBeforeSelf(params object?[] content)
	{
	}

	public IEnumerable<XElement> Ancestors()
	{
		throw null;
	}

	public IEnumerable<XElement> Ancestors(XName? name)
	{
		throw null;
	}

	public static int CompareDocumentOrder(XNode? n1, XNode? n2)
	{
		throw null;
	}

	public XmlReader CreateReader()
	{
		throw null;
	}

	public XmlReader CreateReader(ReaderOptions readerOptions)
	{
		throw null;
	}

	public static bool DeepEquals(XNode? n1, XNode? n2)
	{
		throw null;
	}

	public IEnumerable<XElement> ElementsAfterSelf()
	{
		throw null;
	}

	public IEnumerable<XElement> ElementsAfterSelf(XName? name)
	{
		throw null;
	}

	public IEnumerable<XElement> ElementsBeforeSelf()
	{
		throw null;
	}

	public IEnumerable<XElement> ElementsBeforeSelf(XName? name)
	{
		throw null;
	}

	public bool IsAfter(XNode? node)
	{
		throw null;
	}

	public bool IsBefore(XNode? node)
	{
		throw null;
	}

	public IEnumerable<XNode> NodesAfterSelf()
	{
		throw null;
	}

	public IEnumerable<XNode> NodesBeforeSelf()
	{
		throw null;
	}

	public static XNode ReadFrom(XmlReader reader)
	{
		throw null;
	}

	public static Task<XNode> ReadFromAsync(XmlReader reader, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void Remove()
	{
	}

	public void ReplaceWith(object? content)
	{
	}

	public void ReplaceWith(params object?[] content)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString(SaveOptions options)
	{
		throw null;
	}

	public abstract void WriteTo(XmlWriter writer);

	public abstract Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken);
}
