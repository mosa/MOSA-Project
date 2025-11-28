using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlSingle : INullable, IComparable, IXmlSerializable, IEquatable<SqlSingle>
{
	private int _dummyPrimitive;

	public static readonly SqlSingle MaxValue;

	public static readonly SqlSingle MinValue;

	public static readonly SqlSingle Null;

	public static readonly SqlSingle Zero;

	public bool IsNull
	{
		get
		{
			throw null;
		}
	}

	public float Value
	{
		get
		{
			throw null;
		}
	}

	public SqlSingle(double value)
	{
		throw null;
	}

	public SqlSingle(float value)
	{
		throw null;
	}

	public static SqlSingle Add(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public int CompareTo(SqlSingle value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlSingle Divide(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public bool Equals(SqlSingle other)
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

	public static SqlBoolean GreaterThan(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlSingle Multiply(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlSingle operator +(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlSingle operator /(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static explicit operator SqlSingle(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator SqlSingle(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator float(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlSingle(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static implicit operator SqlSingle(SqlByte x)
	{
		throw null;
	}

	public static implicit operator SqlSingle(SqlDecimal x)
	{
		throw null;
	}

	public static implicit operator SqlSingle(SqlInt16 x)
	{
		throw null;
	}

	public static implicit operator SqlSingle(SqlInt32 x)
	{
		throw null;
	}

	public static implicit operator SqlSingle(SqlInt64 x)
	{
		throw null;
	}

	public static implicit operator SqlSingle(SqlMoney x)
	{
		throw null;
	}

	public static implicit operator SqlSingle(float x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlSingle operator *(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlSingle operator -(SqlSingle x, SqlSingle y)
	{
		throw null;
	}

	public static SqlSingle operator -(SqlSingle x)
	{
		throw null;
	}

	public static SqlSingle Parse(string s)
	{
		throw null;
	}

	public static SqlSingle Subtract(SqlSingle x, SqlSingle y)
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

	public SqlInt64 ToSqlInt64()
	{
		throw null;
	}

	public SqlMoney ToSqlMoney()
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
