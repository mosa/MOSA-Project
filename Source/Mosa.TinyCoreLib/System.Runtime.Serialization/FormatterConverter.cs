namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public class FormatterConverter : IFormatterConverter
{
	public object Convert(object value, Type type)
	{
		throw null;
	}

	public object Convert(object value, TypeCode typeCode)
	{
		throw null;
	}

	public bool ToBoolean(object value)
	{
		throw null;
	}

	public byte ToByte(object value)
	{
		throw null;
	}

	public char ToChar(object value)
	{
		throw null;
	}

	public DateTime ToDateTime(object value)
	{
		throw null;
	}

	public decimal ToDecimal(object value)
	{
		throw null;
	}

	public double ToDouble(object value)
	{
		throw null;
	}

	public short ToInt16(object value)
	{
		throw null;
	}

	public int ToInt32(object value)
	{
		throw null;
	}

	public long ToInt64(object value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public sbyte ToSByte(object value)
	{
		throw null;
	}

	public float ToSingle(object value)
	{
		throw null;
	}

	public string? ToString(object value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ushort ToUInt16(object value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public uint ToUInt32(object value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ulong ToUInt64(object value)
	{
		throw null;
	}
}
