using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlInt16 : INullable, IComparable, IXmlSerializable, IEquatable<SqlInt16>
{
	private int _dummyPrimitive;

	public static readonly SqlInt16 MaxValue;

	public static readonly SqlInt16 MinValue;

	public static readonly SqlInt16 Null;

	public static readonly SqlInt16 Zero;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public short Value
	{
		get
		{
			throw null;
		}
	}

	public SqlInt16(short value)
	{
		throw null;
	}

	public static SqlInt16 Add(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 BitwiseAnd(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 BitwiseOr(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public int CompareTo(SqlInt16 value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlInt16 Divide(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public bool Equals(SqlInt16 other)
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

	public static SqlBoolean GreaterThan(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 Mod(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 Modulus(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 Multiply(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 OnesComplement(SqlInt16 x)
	{
		throw null;
	}

	public static SqlInt16 operator +(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator &(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator |(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator /(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator ^(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator short(SqlInt16 x)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlInt32 x)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlInt64 x)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlMoney x)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlInt16(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static implicit operator SqlInt16(SqlByte x)
	{
		throw null;
	}

	public static implicit operator SqlInt16(short x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator %(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator *(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator ~(SqlInt16 x)
	{
		throw null;
	}

	public static SqlInt16 operator -(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}

	public static SqlInt16 operator -(SqlInt16 x)
	{
		throw null;
	}

	public static SqlInt16 Parse(string s)
	{
		throw null;
	}

	public static SqlInt16 Subtract(SqlInt16 x, SqlInt16 y)
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

	public static SqlInt16 Xor(SqlInt16 x, SqlInt16 y)
	{
		throw null;
	}
}
