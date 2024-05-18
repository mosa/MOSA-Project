using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace System.Web;

public sealed class HttpUtility
{
	[return: NotNullIfNotNull("s")]
	public static string? HtmlAttributeEncode(string? s)
	{
		throw null;
	}

	public static void HtmlAttributeEncode(string? s, TextWriter output)
	{
	}

	[return: NotNullIfNotNull("s")]
	public static string? HtmlDecode(string? s)
	{
		throw null;
	}

	public static void HtmlDecode(string? s, TextWriter output)
	{
	}

	[return: NotNullIfNotNull("value")]
	public static string? HtmlEncode(object? value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("s")]
	public static string? HtmlEncode(string? s)
	{
		throw null;
	}

	public static void HtmlEncode(string? s, TextWriter output)
	{
	}

	public static string JavaScriptStringEncode(string? value)
	{
		throw null;
	}

	public static string JavaScriptStringEncode(string? value, bool addDoubleQuotes)
	{
		throw null;
	}

	public static NameValueCollection ParseQueryString(string query)
	{
		throw null;
	}

	public static NameValueCollection ParseQueryString(string query, Encoding encoding)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static string? UrlDecode(byte[]? bytes, int offset, int count, Encoding e)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static string? UrlDecode(byte[]? bytes, Encoding e)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static string? UrlDecode(string? str)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static string? UrlDecode(string? str, Encoding e)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static byte[]? UrlDecodeToBytes(byte[]? bytes)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static byte[]? UrlDecodeToBytes(byte[]? bytes, int offset, int count)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static byte[]? UrlDecodeToBytes(string? str)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static byte[]? UrlDecodeToBytes(string? str, Encoding e)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static string? UrlEncode(byte[]? bytes)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static string? UrlEncode(byte[]? bytes, int offset, int count)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static string? UrlEncode(string? str)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static string? UrlEncode(string? str, Encoding e)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static byte[]? UrlEncodeToBytes(byte[]? bytes)
	{
		throw null;
	}

	[return: NotNullIfNotNull("bytes")]
	public static byte[]? UrlEncodeToBytes(byte[]? bytes, int offset, int count)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static byte[]? UrlEncodeToBytes(string? str)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static byte[]? UrlEncodeToBytes(string? str, Encoding e)
	{
		throw null;
	}

	[Obsolete("This method produces non-standards-compliant output and has interoperability issues. The preferred alternative is UrlEncode(String).")]
	[return: NotNullIfNotNull("str")]
	public static string? UrlEncodeUnicode(string? str)
	{
		throw null;
	}

	[Obsolete("This method produces non-standards-compliant output and has interoperability issues. The preferred alternative is UrlEncodeToBytes(String).")]
	[return: NotNullIfNotNull("str")]
	public static byte[]? UrlEncodeUnicodeToBytes(string? str)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static string? UrlPathEncode(string? str)
	{
		throw null;
	}
}
