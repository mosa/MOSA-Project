using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlInt32 : INullable, IComparable, IXmlSerializable, IEquatable<SqlInt32>
{
	private int _dummyPrimitive;

	public static readonly SqlInt32 MaxValue;

	public static readonly SqlInt32 MinValue;

	public static readonly SqlInt32 Null;

	public static readonly SqlInt32 Zero;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public int Value
	{
		get
		{
			throw null;
		}
	}

	public SqlInt32(int value)
	{
		throw null;
	}

	public static SqlInt32 Add(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 BitwiseAnd(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 BitwiseOr(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public int CompareTo(SqlInt32 value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlInt32 Divide(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public bool Equals(SqlInt32 other)
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

	public static SqlBoolean GreaterThan(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 Mod(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 Modulus(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 Multiply(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 OnesComplement(SqlInt32 x)
	{
		throw null;
	}

	public static SqlInt32 operator +(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator &(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator |(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator /(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator ^(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static explicit operator SqlInt32(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator SqlInt32(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlInt32(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator int(SqlInt32 x)
	{
		throw null;
	}

	public static explicit operator SqlInt32(SqlInt64 x)
	{
		throw null;
	}

	public static explicit operator SqlInt32(SqlMoney x)
	{
		throw null;
	}

	public static explicit operator SqlInt32(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlInt32(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static implicit operator SqlInt32(SqlByte x)
	{
		throw null;
	}

	public static implicit operator SqlInt32(SqlInt16 x)
	{
		throw null;
	}

	public static implicit operator SqlInt32(int x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator %(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator *(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator ~(SqlInt32 x)
	{
		throw null;
	}

	public static SqlInt32 operator -(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}

	public static SqlInt32 operator -(SqlInt32 x)
	{
		throw null;
	}

	public static SqlInt32 Parse(string s)
	{
		throw null;
	}

	public static SqlInt32 Subtract(SqlInt32 x, SqlInt32 y)
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

	public static SqlInt32 Xor(SqlInt32 x, SqlInt32 y)
	{
		throw null;
	}
}
