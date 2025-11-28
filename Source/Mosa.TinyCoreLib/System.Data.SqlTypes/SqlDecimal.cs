using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlDecimal : INullable, IComparable, IXmlSerializable, IEquatable<SqlDecimal>
{
	private int _dummyPrimitive;

	public static readonly byte MaxPrecision;

	public static readonly byte MaxScale;

	public static readonly SqlDecimal MaxValue;

	public static readonly SqlDecimal MinValue;

	public static readonly SqlDecimal Null;

	public byte[] BinData
	{
		get
		{
			throw null;
		}
	}

	public int[] Data
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

	public bool IsPositive
	{
		get
		{
			throw null;
		}
	}

	public byte Precision
	{
		get
		{
			throw null;
		}
	}

	public byte Scale
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

	public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int data1, int data2, int data3, int data4)
	{
		throw null;
	}

	public SqlDecimal(byte bPrecision, byte bScale, bool fPositive, int[] bits)
	{
		throw null;
	}

	public SqlDecimal(decimal value)
	{
		throw null;
	}

	public SqlDecimal(double dVal)
	{
		throw null;
	}

	public SqlDecimal(int value)
	{
		throw null;
	}

	public SqlDecimal(long value)
	{
		throw null;
	}

	public static SqlDecimal Abs(SqlDecimal n)
	{
		throw null;
	}

	public static SqlDecimal Add(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlDecimal AdjustScale(SqlDecimal n, int digits, bool fRound)
	{
		throw null;
	}

	public static SqlDecimal Ceiling(SqlDecimal n)
	{
		throw null;
	}

	public int CompareTo(SqlDecimal value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlDecimal ConvertToPrecScale(SqlDecimal n, int precision, int scale)
	{
		throw null;
	}

	public static SqlDecimal Divide(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public bool Equals(SqlDecimal other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static SqlDecimal Floor(SqlDecimal n)
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

	public static SqlBoolean GreaterThan(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlDecimal Multiply(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlDecimal operator +(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlDecimal operator /(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static explicit operator SqlDecimal(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator decimal(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlDecimal(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator SqlDecimal(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator SqlDecimal(SqlString x)
	{
		throw null;
	}

	public static explicit operator SqlDecimal(double x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static implicit operator SqlDecimal(SqlByte x)
	{
		throw null;
	}

	public static implicit operator SqlDecimal(SqlInt16 x)
	{
		throw null;
	}

	public static implicit operator SqlDecimal(SqlInt32 x)
	{
		throw null;
	}

	public static implicit operator SqlDecimal(SqlInt64 x)
	{
		throw null;
	}

	public static implicit operator SqlDecimal(SqlMoney x)
	{
		throw null;
	}

	public static implicit operator SqlDecimal(decimal x)
	{
		throw null;
	}

	public static implicit operator SqlDecimal(long x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlDecimal operator *(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlDecimal operator -(SqlDecimal x, SqlDecimal y)
	{
		throw null;
	}

	public static SqlDecimal operator -(SqlDecimal x)
	{
		throw null;
	}

	public static SqlDecimal Parse(string s)
	{
		throw null;
	}

	public static SqlDecimal Power(SqlDecimal n, double exp)
	{
		throw null;
	}

	public static SqlDecimal Round(SqlDecimal n, int position)
	{
		throw null;
	}

	public static SqlInt32 Sign(SqlDecimal n)
	{
		throw null;
	}

	public static SqlDecimal Subtract(SqlDecimal x, SqlDecimal y)
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

	public double ToDouble()
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

	public static SqlDecimal Truncate(SqlDecimal n, int position)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public int WriteTdsValue(Span<uint> destination)
	{
		throw null;
	}
}
