using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlDouble : INullable, IComparable, IXmlSerializable, IEquatable<SqlDouble>
{
	private int _dummyPrimitive;

	public static readonly SqlDouble MaxValue;

	public static readonly SqlDouble MinValue;

	public static readonly SqlDouble Null;

	public static readonly SqlDouble Zero;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public double Value
	{
		get
		{
			throw null;
		}
	}

	public SqlDouble(double value)
	{
		throw null;
	}

	public static SqlDouble Add(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public int CompareTo(SqlDouble value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlDouble Divide(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public bool Equals(SqlDouble other)
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

	public static SqlBoolean GreaterThan(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlDouble Multiply(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlDouble operator +(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlDouble operator /(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static explicit operator SqlDouble(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator double(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator SqlDouble(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static implicit operator SqlDouble(SqlByte x)
	{
		throw null;
	}

	public static implicit operator SqlDouble(SqlDecimal x)
	{
		throw null;
	}

	public static implicit operator SqlDouble(SqlInt16 x)
	{
		throw null;
	}

	public static implicit operator SqlDouble(SqlInt32 x)
	{
		throw null;
	}

	public static implicit operator SqlDouble(SqlInt64 x)
	{
		throw null;
	}

	public static implicit operator SqlDouble(SqlMoney x)
	{
		throw null;
	}

	public static implicit operator SqlDouble(SqlSingle x)
	{
		throw null;
	}

	public static implicit operator SqlDouble(double x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlDouble operator *(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlDouble operator -(SqlDouble x, SqlDouble y)
	{
		throw null;
	}

	public static SqlDouble operator -(SqlDouble x)
	{
		throw null;
	}

	public static SqlDouble Parse(string s)
	{
		throw null;
	}

	public static SqlDouble Subtract(SqlDouble x, SqlDouble y)
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
}
