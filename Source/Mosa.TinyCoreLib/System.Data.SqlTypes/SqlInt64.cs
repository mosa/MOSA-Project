using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlInt64 : INullable, IComparable, IXmlSerializable, IEquatable<SqlInt64>
{
	private int _dummyPrimitive;

	public static readonly SqlInt64 MaxValue;

	public static readonly SqlInt64 MinValue;

	public static readonly SqlInt64 Null;

	public static readonly SqlInt64 Zero;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public long Value
	{
		get
		{
			throw null;
		}
	}

	public SqlInt64(long value)
	{
		throw null;
	}

	public static SqlInt64 Add(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 BitwiseAnd(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 BitwiseOr(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public int CompareTo(SqlInt64 value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlInt64 Divide(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public bool Equals(SqlInt64 other)
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

	public static SqlBoolean GreaterThan(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 Mod(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 Modulus(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 Multiply(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 OnesComplement(SqlInt64 x)
	{
		throw null;
	}

	public static SqlInt64 operator +(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator &(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator |(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator /(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator ^(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static explicit operator SqlInt64(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator SqlInt64(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlInt64(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator long(SqlInt64 x)
	{
		throw null;
	}

	public static explicit operator SqlInt64(SqlMoney x)
	{
		throw null;
	}

	public static explicit operator SqlInt64(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlInt64(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static implicit operator SqlInt64(SqlByte x)
	{
		throw null;
	}

	public static implicit operator SqlInt64(SqlInt16 x)
	{
		throw null;
	}

	public static implicit operator SqlInt64(SqlInt32 x)
	{
		throw null;
	}

	public static implicit operator SqlInt64(long x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator %(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator *(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator ~(SqlInt64 x)
	{
		throw null;
	}

	public static SqlInt64 operator -(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}

	public static SqlInt64 operator -(SqlInt64 x)
	{
		throw null;
	}

	public static SqlInt64 Parse(string s)
	{
		throw null;
	}

	public static SqlInt64 Subtract(SqlInt64 x, SqlInt64 y)
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

	public SqlBoolean ToSqlBoolean()
	{
		throw null;
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

	public static SqlInt64 Xor(SqlInt64 x, SqlInt64 y)
	{
		throw null;
	}
}
