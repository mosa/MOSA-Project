using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class XmlConvert
{
	[return: NotNullIfNotNull("name")]
	public static string? DecodeName(string? name)
	{
		throw null;
	}

	[return: NotNullIfNotNull("name")]
	public static string? EncodeLocalName(string? name)
	{
		throw null;
	}

	[return: NotNullIfNotNull("name")]
	public static string? EncodeName(string? name)
	{
		throw null;
	}

	[return: NotNullIfNotNull("name")]
	public static string? EncodeNmToken(string? name)
	{
		throw null;
	}

	public static bool IsNCNameChar(char ch)
	{
		throw null;
	}

	public static bool IsPublicIdChar(char ch)
	{
		throw null;
	}

	public static bool IsStartNCNameChar(char ch)
	{
		throw null;
	}

	public static bool IsWhitespaceChar(char ch)
	{
		throw null;
	}

	public static bool IsXmlChar(char ch)
	{
		throw null;
	}

	public static bool IsXmlSurrogatePair(char lowChar, char highChar)
	{
		throw null;
	}

	public static bool ToBoolean(string s)
	{
		throw null;
	}

	public static byte ToByte(string s)
	{
		throw null;
	}

	public static char ToChar(string s)
	{
		throw null;
	}

	[Obsolete("Use XmlConvert.ToDateTime() that accepts an XmlDateTimeSerializationMode instead.")]
	public static DateTime ToDateTime(string s)
	{
		throw null;
	}

	public static DateTime ToDateTime(string s, [StringSyntax("DateTimeFormat")] string format)
	{
		throw null;
	}

	public static DateTime ToDateTime(string s, [StringSyntax("DateTimeFormat")] string[] formats)
	{
		throw null;
	}

	public static DateTime ToDateTime(string s, XmlDateTimeSerializationMode dateTimeOption)
	{
		throw null;
	}

	public static DateTimeOffset ToDateTimeOffset(string s)
	{
		throw null;
	}

	public static DateTimeOffset ToDateTimeOffset(string s, [StringSyntax("DateTimeFormat")] string format)
	{
		throw null;
	}

	public static DateTimeOffset ToDateTimeOffset(string s, [StringSyntax("DateTimeFormat")] string[] formats)
	{
		throw null;
	}

	public static decimal ToDecimal(string s)
	{
		throw null;
	}

	public static double ToDouble(string s)
	{
		throw null;
	}

	public static Guid ToGuid(string s)
	{
		throw null;
	}

	public static short ToInt16(string s)
	{
		throw null;
	}

	public static int ToInt32(string s)
	{
		throw null;
	}

	public static long ToInt64(string s)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(string s)
	{
		throw null;
	}

	public static float ToSingle(string s)
	{
		throw null;
	}

	public static string ToString(bool value)
	{
		throw null;
	}

	public static string ToString(byte value)
	{
		throw null;
	}

	public static string ToString(char value)
	{
		throw null;
	}

	[Obsolete("Use XmlConvert.ToString() that accepts an XmlDateTimeSerializationMode instead.")]
	public static string ToString(DateTime value)
	{
		throw null;
	}

	public static string ToString(DateTime value, [StringSyntax("DateTimeFormat")] string format)
	{
		throw null;
	}

	public static string ToString(DateTime value, XmlDateTimeSerializationMode dateTimeOption)
	{
		throw null;
	}

	public static string ToString(DateTimeOffset value)
	{
		throw null;
	}

	public static string ToString(DateTimeOffset value, [StringSyntax("DateTimeFormat")] string format)
	{
		throw null;
	}

	public static string ToString(decimal value)
	{
		throw null;
	}

	public static string ToString(double value)
	{
		throw null;
	}

	public static string ToString(Guid value)
	{
		throw null;
	}

	public static string ToString(short value)
	{
		throw null;
	}

	public static string ToString(int value)
	{
		throw null;
	}

	public static string ToString(long value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(sbyte value)
	{
		throw null;
	}

	public static string ToString(float value)
	{
		throw null;
	}

	public static string ToString(TimeSpan value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static string ToString(ulong value)
	{
		throw null;
	}

	public static TimeSpan ToTimeSpan(string s)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(string s)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(string s)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(string s)
	{
		throw null;
	}

	public static string VerifyName(string name)
	{
		throw null;
	}

	public static string VerifyNCName(string name)
	{
		throw null;
	}

	public static string VerifyNMTOKEN(string name)
	{
		throw null;
	}

	public static string VerifyPublicId(string publicId)
	{
		throw null;
	}

	[return: NotNullIfNotNull("token")]
	public static string? VerifyTOKEN(string? token)
	{
		throw null;
	}

	public static string VerifyWhitespace(string content)
	{
		throw null;
	}

	public static string VerifyXmlChars(string content)
	{
		throw null;
	}
}
