using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlBoolean : INullable, IComparable, IXmlSerializable, IEquatable<SqlBoolean>
{
	private int _dummyPrimitive;

	public static readonly SqlBoolean False;

	public static readonly SqlBoolean Null;

	public static readonly SqlBoolean One;

	public static readonly SqlBoolean True;

	public static readonly SqlBoolean Zero;

	public byte ByteValue
	{
		get
		{
			throw null;
		}
	}

	public bool IsFalse
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

	public bool IsTrue
	{
		get
		{
			throw null;
		}
	}

	public bool Value
	{
		get
		{
			throw null;
		}
	}

	public SqlBoolean(bool value)
	{
		throw null;
	}

	public SqlBoolean(int value)
	{
		throw null;
	}

	public static SqlBoolean And(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public int CompareTo(SqlBoolean value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public bool Equals(SqlBoolean other)
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

	public static SqlBoolean GreaterThan(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEquals(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEquals(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean OnesComplement(SqlBoolean x)
	{
		throw null;
	}

	public static SqlBoolean operator &(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean operator |(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean operator ^(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static explicit operator bool(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlByte x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlInt16 x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlInt32 x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlInt64 x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlMoney x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlBoolean(SqlString x)
	{
		throw null;
	}

	public static bool operator false(SqlBoolean x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static implicit operator SqlBoolean(bool x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean operator !(SqlBoolean x)
	{
		throw null;
	}

	public static SqlBoolean operator ~(SqlBoolean x)
	{
		throw null;
	}

	public static bool operator true(SqlBoolean x)
	{
		throw null;
	}

	public static SqlBoolean Or(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}

	public static SqlBoolean Parse(string s)
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

	public SqlByte ToSqlByte()
	{
		throw null;
	}

	public SqlDecimal ToSqlDecimal()
	{
		throw null;
	}

	public SqlDouble ToSqlDouble()
	{
		throw null;
	}

	public SqlInt16 ToSqlInt16()
	{
		throw null;
	}

	public SqlInt32 ToSqlInt32()
	{
		throw null;
	}

	public SqlInt64 ToSqlInt64()
	{
		throw null;
	}

	public SqlMoney ToSqlMoney()
	{
		throw null;
	}

	public SqlSingle ToSqlSingle()
	{
		throw null;
	}

	public SqlString ToSqlString()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static SqlBoolean Xor(SqlBoolean x, SqlBoolean y)
	{
		throw null;
	}
}
