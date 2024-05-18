using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlString : INullable, IComparable, IXmlSerializable, IEquatable<SqlString>
{
	private object _dummy;

	private int _dummyPrimitive;

	public static readonly int BinarySort;

	public static readonly int BinarySort2;

	public static readonly int IgnoreCase;

	public static readonly int IgnoreKanaType;

	public static readonly int IgnoreNonSpace;

	public static readonly int IgnoreWidth;

	public static readonly SqlString Null;

	public CompareInfo CompareInfo
	{
		get
		{
			throw null;
		}
	}

	public CultureInfo CultureInfo
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

	public int LCID
	{
		get
		{
			throw null;
		}
	}

	public SqlCompareOptions SqlCompareOptions
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
	}

	public SqlString(int lcid, SqlCompareOptions compareOptions, byte[] data)
	{
		throw null;
	}

	public SqlString(int lcid, SqlCompareOptions compareOptions, byte[] data, bool fUnicode)
	{
		throw null;
	}

	public SqlString(int lcid, SqlCompareOptions compareOptions, byte[]? data, int index, int count)
	{
		throw null;
	}

	public SqlString(int lcid, SqlCompareOptions compareOptions, byte[]? data, int index, int count, bool fUnicode)
	{
		throw null;
	}

	public SqlString(string? data)
	{
		throw null;
	}

	public SqlString(string? data, int lcid)
	{
		throw null;
	}

	public SqlString(string? data, int lcid, SqlCompareOptions compareOptions)
	{
		throw null;
	}

	public static SqlString Add(SqlString x, SqlString y)
	{
		throw null;
	}

	public SqlString Clone()
	{
		throw null;
	}

	public static CompareOptions CompareOptionsFromSqlCompareOptions(SqlCompareOptions compareOptions)
	{
		throw null;
	}

	public int CompareTo(SqlString value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlString Concat(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlString x, SqlString y)
	{
		throw null;
	}

	public bool Equals(SqlString other)
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

	public byte[]? GetNonUnicodeBytes()
	{
		throw null;
	}

	public byte[]? GetUnicodeBytes()
	{
		throw null;
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		throw null;
	}

	public static SqlBoolean GreaterThan(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlString operator +(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlString x, SqlString y)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlBoolean x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlByte x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlDateTime x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlDecimal x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlDouble x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlGuid x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlInt16 x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlInt32 x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlInt64 x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlMoney x)
	{
		throw null;
	}

	public static explicit operator SqlString(SqlSingle x)
	{
		throw null;
	}

	public static explicit operator string(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlString x, SqlString y)
	{
		throw null;
	}

	public static implicit operator SqlString(string x)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlString x, SqlString y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlString x, SqlString y)
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

	public SqlByte ToSqlByte()
	{
		throw null;
	}

	public SqlDateTime ToSqlDateTime()
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

	public SqlGuid ToSqlGuid()
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

	public override string ToString()
	{
		throw null;
	}
}
