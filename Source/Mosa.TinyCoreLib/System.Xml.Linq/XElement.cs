using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Xml.Linq;

[TypeDescriptionProvider("MS.Internal.Xml.Linq.ComponentModel.XTypeDescriptionProvider`1[[System.Xml.Linq.XElement, System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],System.ComponentModel.TypeConverter")]
[XmlSchemaProvider(null, IsAny = true)]
public class XElement : XContainer, IXmlSerializable
{
	public static IEnumerable<XElement> EmptySequence
	{
		get
		{
			throw null;
		}
	}

	public XAttribute? FirstAttribute
	{
		get
		{
			throw null;
		}
	}

	public bool HasAttributes
	{
		get
		{
			throw null;
		}
	}

	public bool HasElements
	{
		get
		{
			throw null;
		}
	}

	public bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public XAttribute? LastAttribute
	{
		get
		{
			throw null;
		}
	}

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

	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XElement(XElement other)
	{
	}

	public XElement(XName name)
	{
	}

	public XElement(XName name, object? content)
	{
	}

	public XElement(XName name, params object?[] content)
	{
	}

	public XElement(XStreamingElement other)
	{
	}

	public IEnumerable<XElement> AncestorsAndSelf()
	{
		throw null;
	}

	public IEnumerable<XElement> AncestorsAndSelf(XName? name)
	{
		throw null;
	}

	public XAttribute? Attribute(XName name)
	{
		throw null;
	}

	public IEnumerable<XAttribute> Attributes()
	{
		throw null;
	}

	public IEnumerable<XAttribute> Attributes(XName? name)
	{
		throw null;
	}

	public IEnumerable<XNode> DescendantNodesAndSelf()
	{
		throw null;
	}

	public IEnumerable<XElement> DescendantsAndSelf()
	{
		throw null;
	}

	public IEnumerable<XElement> DescendantsAndSelf(XName? name)
	{
		throw null;
	}

	public XNamespace GetDefaultNamespace()
	{
		throw null;
	}

	public XNamespace? GetNamespaceOfPrefix(string prefix)
	{
		throw null;
	}

	public string? GetPrefixOfNamespace(XNamespace ns)
	{
		throw null;
	}

	public static XElement Load(Stream stream)
	{
		throw null;
	}

	public static XElement Load(Stream stream, LoadOptions options)
	{
		throw null;
	}

	public static XElement Load(TextReader textReader)
	{
		throw null;
	}

	public static XElement Load(TextReader textReader, LoadOptions options)
	{
		throw null;
	}

	public static XElement Load([StringSyntax("Uri")] string uri)
	{
		throw null;
	}

	public static XElement Load([StringSyntax("Uri")] string uri, LoadOptions options)
	{
		throw null;
	}

	public static XElement Load(XmlReader reader)
	{
		throw null;
	}

	public static XElement Load(XmlReader reader, LoadOptions options)
	{
		throw null;
	}

	public static Task<XElement> LoadAsync(Stream stream, LoadOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<XElement> LoadAsync(TextReader textReader, LoadOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<XElement> LoadAsync(XmlReader reader, LoadOptions options, CancellationToken cancellationToken)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator bool(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator DateTime(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator DateTimeOffset(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator decimal(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator double(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Guid(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator int(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator long(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator bool?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator DateTimeOffset?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator DateTime?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator decimal?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator double?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator Guid?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator int?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator long?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator float?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator TimeSpan?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator uint?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator ulong?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator float(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("element")]
	public static explicit operator string?(XElement? element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator TimeSpan(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(XElement element)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(XElement element)
	{
		throw null;
	}

	public static XElement Parse(string text)
	{
		throw null;
	}

	public static XElement Parse(string text, LoadOptions options)
	{
		throw null;
	}

	public void RemoveAll()
	{
	}

	public void RemoveAttributes()
	{
	}

	public void ReplaceAll(object? content)
	{
	}

	public void ReplaceAll(params object?[] content)
	{
	}

	public void ReplaceAttributes(object? content)
	{
	}

	public void ReplaceAttributes(params object?[] content)
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

	public void SetAttributeValue(XName name, object? value)
	{
	}

	public void SetElementValue(XName name, object? value)
	{
	}

	public void SetValue(object value)
	{
	}

	XmlSchema IXmlSerializable.GetSchema()
	{
		throw null;
	}

	void IXmlSerializable.ReadXml(XmlReader reader)
	{
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
	}

	public override void WriteTo(XmlWriter writer)
	{
	}

	public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken)
	{
		throw null;
	}
}
