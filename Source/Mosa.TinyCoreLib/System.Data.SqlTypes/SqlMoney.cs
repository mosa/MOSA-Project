using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlMoney : INullable, IComparable, IXmlSerializable, IEquatable<SqlMoney>
{
	private int _dummyPrimitive;

	public static readonly SqlMoney MaxValue;

	public static readonly SqlMoney MinValue;

	public static readonly SqlMoney Null;

	public static readonly SqlMoney Zero;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public decimal Value
	{
		get
		{
			throw null;
		}
	}

	public SqlMoney(decimal value)
	{
		throw null;
	}

	public SqlMoney(double value)
	{
		throw null;
	}

	public SqlMoney(int value)
	{
		throw null;
	}

	public SqlMoney(long value)
	{
		throw null;
	}

	public static SqlMoney Add(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public int CompareTo(SqlMoney value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlMoney Divide(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public bool Equals(SqlMoney other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static SqlMoney FromTdsValue(long value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public long GetTdsValue()
	{
		throw null;
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		throw null;
	}

	public static SqlBoolean GreaterThan(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlMoney Multiply(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlMoney operator +(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlMoney operator /(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static explicit operator SqlMoney(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator SqlMoney(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlMoney(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator decimal(SqlMoney x)
	{
		throw null;
	}

	public static explicit operator SqlMoney(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlMoney(SqlString x)
	{
		throw null;
	}

	public static explicit operator SqlMoney(double x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static implicit operator SqlMoney(SqlByte x)
	{
		throw null;
	}

	public static implicit operator SqlMoney(SqlInt16 x)
	{
		throw null;
	}

	public static implicit operator SqlMoney(SqlInt32 x)
	{
		throw null;
	}

	public static implicit operator SqlMoney(SqlInt64 x)
	{
		throw null;
	}

	public static implicit operator SqlMoney(decimal x)
	{
		throw null;
	}

	public static implicit operator SqlMoney(long x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlMoney operator *(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlMoney operator -(SqlMoney x, SqlMoney y)
	{
		throw null;
	}

	public static SqlMoney operator -(SqlMoney x)
	{
		throw null;
	}

	public static SqlMoney Parse(string s)
	{
		throw null;
	}

	public static SqlMoney Subtract(SqlMoney x, SqlMoney y)
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

	public decimal ToDecimal()
	{
		throw null;
	}

	public double ToDouble()
	{
		throw null;
	}

	public int ToInt32()
	{
		throw null;
	}

	public long ToInt64()
	{
		throw null;
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

	public SqlInt64 ToSqlInt64()
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
}
