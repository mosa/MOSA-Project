using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[XmlSchemaProvider("GetXsdType")]
public struct SqlDateTime : INullable, IComparable, IXmlSerializable, IEquatable<SqlDateTime>
{
	private int _dummyPrimitive;

	public static readonly SqlDateTime MaxValue;

	public static readonly SqlDateTime MinValue;

	public static readonly SqlDateTime Null;

	public static readonly int SQLTicksPerHour;

	public static readonly int SQLTicksPerMinute;

	public static readonly int SQLTicksPerSecond;

	public int DayTicks
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

	public int TimeTicks
	{
		get
		{
			throw null;
		}
	}

	public DateTime Value
	{
		get
		{
			throw null;
		}
	}

	public SqlDateTime(DateTime value)
	{
		throw null;
	}

	public SqlDateTime(int dayTicks, int timeTicks)
	{
		throw null;
	}

	public SqlDateTime(int year, int month, int day)
	{
		throw null;
	}

	public SqlDateTime(int year, int month, int day, int hour, int minute, int second)
	{
		throw null;
	}

	public SqlDateTime(int year, int month, int day, int hour, int minute, int second, double millisecond)
	{
		throw null;
	}

	public SqlDateTime(int year, int month, int day, int hour, int minute, int second, int bilisecond)
	{
		throw null;
	}

	public static SqlDateTime Add(SqlDateTime x, TimeSpan t)
	{
		throw null;
	}

	public int CompareTo(SqlDateTime value)
	{
		throw null;
	}

	public int CompareTo(object? value)
	{
		throw null;
	}

	public static SqlBoolean Equals(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public bool Equals(SqlDateTime other)
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

	public static SqlBoolean GreaterThan(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlBoolean GreaterThanOrEqual(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlBoolean LessThan(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlBoolean LessThanOrEqual(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlBoolean NotEquals(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlDateTime operator +(SqlDateTime x, TimeSpan t)
	{
		throw null;
	}

	public static SqlBoolean operator ==(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static explicit operator DateTime(SqlDateTime x)
	{
		throw null;
	}

	public static explicit operator SqlDateTime(SqlString x)
	{
		throw null;
	}

	public static SqlBoolean operator >(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlBoolean operator >=(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static implicit operator SqlDateTime(DateTime value)
	{
		throw null;
	}

	public static SqlBoolean operator !=(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlBoolean operator <(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlBoolean operator <=(SqlDateTime x, SqlDateTime y)
	{
		throw null;
	}

	public static SqlDateTime operator -(SqlDateTime x, TimeSpan t)
	{
		throw null;
	}

	public static SqlDateTime Parse(string s)
	{
		throw null;
	}

	public static SqlDateTime Subtract(SqlDateTime x, TimeSpan t)
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

	public SqlString ToSqlString()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
