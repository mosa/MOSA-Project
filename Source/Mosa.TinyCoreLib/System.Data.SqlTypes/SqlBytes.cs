using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public sealed class SqlBytes : INullable, ISerializable, IXmlSerializable
{
	public byte[]? Buffer
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

	public byte this[long offset]
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

	public static SqlBytes Null
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

	public Stream Stream
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] Value
	{
		get
		{
			throw null;
		}
	}

	public SqlBytes()
	{
	}

	public SqlBytes(byte[]? buffer)
	{
	}

	public SqlBytes(SqlBinary value)
	{
	}

	public SqlBytes(Stream? s)
	{
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		throw null;
	}

	public static explicit operator SqlBytes(SqlBinary value)
	{
		throw null;
	}

	public static explicit operator SqlBinary(SqlBytes value)
	{
		throw null;
	}

	public long Read(long offset, byte[] buffer, int offsetInBuffer, int count)
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

	public SqlBinary ToSqlBinary()
	{
		throw null;
	}

	public void Write(long offset, byte[] buffer, int offsetInBuffer, int count)
	{
	}
}
