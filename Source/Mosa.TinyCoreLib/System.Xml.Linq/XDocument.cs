using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

public class XDocument : XContainer
{
	public XDeclaration? Declaration
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XDocumentType? DocumentType
	{
		get
		{
			throw null;
		}
	}

	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public XElement? Root
	{
		get
		{
			throw null;
		}
	}

	public XDocument()
	{
	}

	public XDocument(params object?[] content)
	{
	}

	public XDocument(XDeclaration? declaration, params object?[] content)
	{
	}

	public XDocument(XDocument other)
	{
	}

	public static XDocument Load(Stream stream)
	{
		throw null;
	}

	public static XDocument Load(Stream stream, LoadOptions options)
	{
		throw null;
	}

	public static XDocument Load(TextReader textReader)
	{
		throw null;
	}

	public static XDocument Load(TextReader textReader, LoadOptions options)
	{
		throw null;
	}

	public static XDocument Load([StringSyntax("Uri")] string uri)
	{
		throw null;
	}

	public static XDocument Load([StringSyntax("Uri")] string uri, LoadOptions options)
	{
		throw null;
	}

	public static XDocument Load(XmlReader reader)
	{
		throw null;
	}

	public static XDocument Load(XmlReader reader, LoadOptions options)
	{
		throw null;
	}

	public static Task<XDocument> LoadAsync(Stream stream, LoadOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<XDocument> LoadAsync(TextReader textReader, LoadOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<XDocument> LoadAsync(XmlReader reader, LoadOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static XDocument Parse(string text)
	{
		throw null;
	}

	public static XDocument Parse(string text, LoadOptions options)
	{
		throw null;
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

	public Task SaveAsync(Stream stream, SaveOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task SaveAsync(TextWriter textWriter, SaveOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task SaveAsync(XmlWriter writer, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override void WriteTo(XmlWriter writer)
	{
	}

	public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken)
	{
		throw null;
	}
}
