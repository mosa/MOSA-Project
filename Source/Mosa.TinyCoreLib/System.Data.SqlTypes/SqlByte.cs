using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlByte : INullable, IComparable, IXmlSerializable, IEquatable<SqlByte>
{
	private int _dummyPrimitive;

	public static readonly SqlByte MaxValue;

	public static readonly SqlByte MinValue;

	public static readonly SqlByte Null;

	public static readonly SqlByte Zero;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public byte Value
	{
		get
		{
			throw null;
		}
	}

	public SqlByte(byte value)
	{
		throw null;
	}

	public static SqlByte Add(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte BitwiseAnd(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte BitwiseOr(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public int CompareTo(SqlByte value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlByte Divide(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public bool Equals(SqlByte other)
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

	public static SqlBoolean GreaterThan(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte Mod(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte Modulus(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte Multiply(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte OnesComplement(SqlByte x)
	{
		throw null;
	}

	public static SqlByte operator +(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte operator &(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte operator |(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte operator /(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte operator ^(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator byte(SqlByte x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlInt16 x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlInt32 x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlInt64 x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlMoney x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlByte(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static implicit operator SqlByte(byte x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte operator %(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte operator *(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte operator ~(SqlByte x)
	{
		throw null;
	}

	public static SqlByte operator -(SqlByte x, SqlByte y)
	{
		throw null;
	}

	public static SqlByte Parse(string s)
	{
		throw null;
	}

	public static SqlByte Subtract(SqlByte x, SqlByte y)
	{
		throw null;
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

	public SqlBoolean ToSqlBoolean()
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

	public static SqlByte Xor(SqlByte x, SqlByte y)
	{
		throw null;
	}
}
