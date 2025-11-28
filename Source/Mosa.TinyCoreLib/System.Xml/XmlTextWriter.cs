using System.ComponentModel;
using System.IO;
using System.Text;

namespace System.Xml;

[EditorBrowsable(EditorBrowsableState.Never)]
public class XmlTextWriter : XmlWriter
{
	public Stream? BaseStream
	{
		get
		{
			throw null;
		}
	}

	public Formatting Formatting
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Indentation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public char IndentChar
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Namespaces
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public char QuoteChar
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override WriteState WriteState
	{
		get
		{
			throw null;
		}
	}

	public override string? XmlLang
	{
		get
		{
			throw null;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			throw null;
		}
	}

	public XmlTextWriter(Stream w, Encoding? encoding)
	{
	}

	public XmlTextWriter(TextWriter w)
	{
	}

	public XmlTextWriter(string filename, Encoding? encoding)
	{
	}

	public override void Close()
	{
	}

	public override void Flush()
	{
	}

	public override string? LookupPrefix(string ns)
	{
		throw null;
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
	}

	public override void WriteBinHex(byte[] buffer, int index, int count)
	{
	}

	public override void WriteCData(string? text)
	{
	}

	public override void WriteCharEntity(char ch)
	{
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
	}

	public override void WriteComment(string? text)
	{
	}

	public override void WriteDocType(string name, string? pubid, string? sysid, string? subset)
	{
	}

	public override void WriteEndAttribute()
	{
	}

	public override void WriteEndDocument()
	{
	}

	public override void WriteEndElement()
	{
	}

	public override void WriteEntityRef(string name)
	{
	}

	public override void WriteFullEndElement()
	{
	}

	public override void WriteName(string name)
	{
	}

	public override void WriteNmToken(string name)
	{
	}

	public override void WriteProcessingInstruction(string name, string? text)
	{
	}

	public override void WriteQualifiedName(string localName, string? ns)
	{
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
	}

	public override void WriteRaw(string data)
	{
	}

	public override void WriteStartAttribute(string? prefix, string localName, string? ns)
	{
	}

	public override void WriteStartDocument()
	{
	}

	public override void WriteStartDocument(bool standalone)
	{
	}

	public override void WriteStartElement(string? prefix, string localName, string? ns)
	{
	}

	public override void WriteString(string? text)
	{
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
	}

	public override void WriteWhitespace(string? ws)
	{
	}
}
