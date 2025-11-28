using System.Collections.Generic;

namespace System.Xml.Linq;

public abstract class XObject : IXmlLineInfo
{
	public string BaseUri
	{
		get
		{
			throw null;
		}
	}

	public XDocument? Document
	{
		get
		{
			throw null;
		}
	}

	public abstract XmlNodeType NodeType { get; }

	public XElement? Parent
	{
		get
		{
			throw null;
		}
	}

	int IXmlLineInfo.LineNumber
	{
		get
		{
			throw null;
		}
	}

	int IXmlLineInfo.LinePosition
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler<XObjectChangeEventArgs> Changed
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<XObjectChangeEventArgs> Changing
	{
		add
		{
		}
		remove
		{
		}
	}

	internal XObject()
	{
	}

	public void AddAnnotation(object annotation)
	{
	}

	public object? Annotation(Type type)
	{
		throw null;
	}

	public IEnumerable<object> Annotations(Type type)
	{
		throw null;
	}

	public IEnumerable<T> Annotations<T>() where T : class
	{
		throw null;
	}

	public T? Annotation<T>() where T : class
	{
		throw null;
	}

	public void RemoveAnnotations(Type type)
	{
	}

	public void RemoveAnnotations<T>() where T : class
	{
	}

	bool IXmlLineInfo.HasLineInfo()
	{
		throw null;
	}
}
