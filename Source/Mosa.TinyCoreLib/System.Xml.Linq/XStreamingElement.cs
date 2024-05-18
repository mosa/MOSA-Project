using System.IO;

namespace System.Xml.Linq;

public class XStreamingElement
{
	public XName Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XStreamingElement(XName name)
	{
	}

	public XStreamingElement(XName name, object? content)
	{
	}

	public XStreamingElement(XName name, params object?[] content)
	{
	}

	public void Add(object? content)
	{
	}

	public void Add(params object?[] content)
	{
	}

	public void Save(Stream stream)
	{
	}

	public void Save(Stream stream, SaveOptions options)
	{
	}

	public void Save(TextWriter textWriter)
	{
	}

	public void Save(TextWriter textWriter, SaveOptions options)
	{
	}

	public void Save(string fileName)
	{
	}

	public void Save(string fileName, SaveOptions options)
	{
	}

	public void Save(XmlWriter writer)
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

	public void WriteTo(XmlWriter writer)
	{
	}
}
