using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlBinary : INullable, IComparable, IXmlSerializable, IEquatable<SqlBinary>
{
	private object _dummy;

	private int _dummyPrimitive;

	public static readonly SqlBinary Null;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public byte this[int index]
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public byte[] Value
	{
		get
		{
			throw null;
		}
	}

	public SqlBinary(byte[]? value)
	{
		throw null;
	}

	public static SqlBinary Add(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public int CompareTo(SqlBinary value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlBinary Concat(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public bool Equals(SqlBinary other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		throw null;
	}

	public static SqlBoolean GreaterThan(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBinary operator +(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static explicit operator byte[]?(SqlBinary x)
	{
		throw null;
	}

	public static explicit operator SqlBinary(SqlGuid x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static implicit operator SqlBinary(byte[] x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlBinary x, SqlBinary y)
	{
		throw null;
	}

	XmlSchema? IXmlSerializable.GetSchema()
	{
		throw null;
	}

	void IXmlSerializable.ReadXml(XmlReader reader)
	{
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
	}

	public SqlGuid ToSqlGuid()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static SqlBinary WrapBytes(byte[] bytes)
	{
		throw null;
	}
}
