using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace System.Xml;

public abstract class XmlWriter : IAsyncDisposable, IDisposable
{
	public virtual XmlWriterSettings? Settings
	{
		get
		{
			throw null;
		}
	}

	public abstract WriteState WriteState { get; }

	public virtual string? XmlLang
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlSpace XmlSpace
	{
		get
		{
			throw null;
		}
	}

	public virtual void Close()
	{
	}

	public static XmlWriter Create(Stream output)
	{
		throw null;
	}

	public static XmlWriter Create(Stream output, XmlWriterSettings? settings)
	{
		throw null;
	}

	public static XmlWriter Create(TextWriter output)
	{
		throw null;
	}

	public static XmlWriter Create(TextWriter output, XmlWriterSettings? settings)
	{
		throw null;
	}

	public static XmlWriter Create(string outputFileName)
	{
		throw null;
	}

	public static XmlWriter Create(string outputFileName, XmlWriterSettings? settings)
	{
		throw null;
	}

	public static XmlWriter Create(StringBuilder output)
	{
		throw null;
	}

	public static XmlWriter Create(StringBuilder output, XmlWriterSettings? settings)
	{
		throw null;
	}

	public static XmlWriter Create(XmlWriter output)
	{
		throw null;
	}

	public static XmlWriter Create(XmlWriter output, XmlWriterSettings? settings)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract void Flush();

	public virtual Task FlushAsync()
	{
		throw null;
	}

	public abstract string? LookupPrefix(string ns);

	public virtual void WriteAttributes(XmlReader reader, bool defattr)
	{
	}

	[DebuggerStepThrough]
	public virtual Task WriteAttributesAsync(XmlReader reader, bool defattr)
	{
		throw null;
	}

	public void WriteAttributeString(string localName, string? value)
	{
	}

	public void WriteAttributeString(string localName, string? ns, string? value)
	{
	}

	public void WriteAttributeString(string? prefix, string localName, string? ns, string? value)
	{
	}

	public Task WriteAttributeStringAsync(string? prefix, string localName, string? ns, string? value)
	{
		throw null;
	}

	public abstract void WriteBase64(byte[] buffer, int index, int count);

	public virtual Task WriteBase64Async(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual void WriteBinHex(byte[] buffer, int index, int count)
	{
	}

	public virtual Task WriteBinHexAsync(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public abstract void WriteCData(string? text);

	public virtual Task WriteCDataAsync(string? text)
	{
		throw null;
	}

	public abstract void WriteCharEntity(char ch);

	public virtual Task WriteCharEntityAsync(char ch)
	{
		throw null;
	}

	public abstract void WriteChars(char[] buffer, int index, int count);

	public virtual Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public abstract void WriteComment(string? text);

	public virtual Task WriteCommentAsync(string? text)
	{
		throw null;
	}

	public abstract void WriteDocType(string name, string? pubid, string? sysid, string? subset);

	public virtual Task WriteDocTypeAsync(string name, string? pubid, string? sysid, string? subset)
	{
		throw null;
	}

	public void WriteElementString(string localName, string? value)
	{
	}

	public void WriteElementString(string localName, string? ns, string? value)
	{
	}

	public void WriteElementString(string? prefix, string localName, string? ns, string? value)
	{
	}

	[DebuggerStepThrough]
	public Task WriteElementStringAsync(string? prefix, string localName, string? ns, string value)
	{
		throw null;
	}

	public abstract void WriteEndAttribute();

	protected internal virtual Task WriteEndAttributeAsync()
	{
		throw null;
	}

	public abstract void WriteEndDocument();

	public virtual Task WriteEndDocumentAsync()
	{
		throw null;
	}

	public abstract void WriteEndElement();

	public virtual Task WriteEndElementAsync()
	{
		throw null;
	}

	public abstract void WriteEntityRef(string name);

	public virtual Task WriteEntityRefAsync(string name)
	{
		throw null;
	}

	public abstract void WriteFullEndElement();

	public virtual Task WriteFullEndElementAsync()
	{
		throw null;
	}

	public virtual void WriteName(string name)
	{
	}

	public virtual Task WriteNameAsync(string name)
	{
		throw null;
	}

	public virtual void WriteNmToken(string name)
	{
	}

	public virtual Task WriteNmTokenAsync(string name)
	{
		throw null;
	}

	public virtual void WriteNode(XmlReader reader, bool defattr)
	{
	}

	public virtual void WriteNode(XPathNavigator navigator, bool defattr)
	{
	}

	public virtual Task WriteNodeAsync(XmlReader reader, bool defattr)
	{
		throw null;
	}

	[DebuggerStepThrough]
	public virtual Task WriteNodeAsync(XPathNavigator navigator, bool defattr)
	{
		throw null;
	}

	public abstract void WriteProcessingInstruction(string name, string? text);

	public virtual Task WriteProcessingInstructionAsync(string name, string? text)
	{
		throw null;
	}

	public virtual void WriteQualifiedName(string localName, string? ns)
	{
	}

	[DebuggerStepThrough]
	public virtual Task WriteQualifiedNameAsync(string localName, string? ns)
	{
		throw null;
	}

	public abstract void WriteRaw(char[] buffer, int index, int count);

	public abstract void WriteRaw(string data);

	public virtual Task WriteRawAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task WriteRawAsync(string data)
	{
		throw null;
	}

	public void WriteStartAttribute(string localName)
	{
	}

	public void WriteStartAttribute(string localName, string? ns)
	{
	}

	public abstract void WriteStartAttribute(string? prefix, string localName, string? ns);

	protected internal virtual Task WriteStartAttributeAsync(string? prefix, string localName, string? ns)
	{
		throw null;
	}

	public abstract void WriteStartDocument();

	public abstract void WriteStartDocument(bool standalone);

	public virtual Task WriteStartDocumentAsync()
	{
		throw null;
	}

	public virtual Task WriteStartDocumentAsync(bool standalone)
	{
		throw null;
	}

	public void WriteStartElement(string localName)
	{
	}

	public void WriteStartElement(string localName, string? ns)
	{
	}

	public abstract void WriteStartElement(string? prefix, string localName, string? ns);

	public virtual Task WriteStartElementAsync(string? prefix, string localName, string? ns)
	{
		throw null;
	}

	public abstract void WriteString(string? text);

	public virtual Task WriteStringAsync(string? text)
	{
		throw null;
	}

	public abstract void WriteSurrogateCharEntity(char lowChar, char highChar);

	public virtual Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		throw null;
	}

	public virtual void WriteValue(bool value)
	{
	}

	public virtual void WriteValue(DateTime value)
	{
	}

	public virtual void WriteValue(DateTimeOffset value)
	{
	}

	public virtual void WriteValue(decimal value)
	{
	}

	public virtual void WriteValue(double value)
	{
	}

	public virtual void WriteValue(int value)
	{
	}

	public virtual void WriteValue(long value)
	{
	}

	public virtual void WriteValue(object value)
	{
	}

	public virtual void WriteValue(float value)
	{
	}

	public virtual void WriteValue(string? value)
	{
	}

	public abstract void WriteWhitespace(string? ws);

	public virtual Task WriteWhitespaceAsync(string? ws)
	{
		throw null;
	}

	public ValueTask DisposeAsync()
	{
		throw null;
	}

	protected virtual ValueTask DisposeAsyncCore()
	{
		throw null;
	}
}
