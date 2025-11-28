using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public sealed class SqlChars : INullable, ISerializable, IXmlSerializable
{
	public char[]? Buffer
	{
		get
		{
			throw null;
		}
	}

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public char this[long offset]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long Length
	{
		get
		{
			throw null;
		}
	}

	public long MaxLength
	{
		get
		{
			throw null;
		}
	}

	public static SqlChars Null
	{
		get
		{
			throw null;
		}
	}

	public StorageState Storage
	{
		get
		{
			throw null;
		}
	}

	public char[] Value
	{
		get
		{
			throw null;
		}
	}

	public SqlChars()
	{
	}

	public SqlChars(char[]? buffer)
	{
	}

	public SqlChars(SqlString value)
	{
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlChars value)
	{
		throw null;
	}

	public static explicit operator SqlChars(SqlString value)
	{
		throw null;
	}

	public long Read(long offset, char[] buffer, int offsetInBuffer, int count)
	{
		throw null;
	}

	public void SetLength(long value)
	{
	}

	public void SetNull()
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	XmlSchema? IXmlSerializable.GetSchema()
	{
		throw null;
	}

	void IXmlSerializable.ReadXml(XmlReader r)
	{
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
	}

	public SqlString ToSqlString()
	{
		throw null;
	}

	public void Write(long offset, char[] buffer, int offsetInBuffer, int count)
	{
	}
}
