using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Net;

public static class WebUtility
{
	[return: NotNullIfNotNull("value")]
	public static string? HtmlDecode(string? value)
	{
		throw null;
	}

	public static void HtmlDecode(string? value, TextWriter output)
	{
	}

	[return: NotNullIfNotNull("value")]
	public static string? HtmlEncode(string? value)
	{
		throw null;
	}

	public static void HtmlEncode(string? value, TextWriter output)
	{
	}

	[return: NotNullIfNotNull("encodedValue")]
	public static string? UrlDecode(string? encodedValue)
	{
		throw null;
	}

	[return: NotNullIfNotNull("encodedValue")]
	public static byte[]? UrlDecodeToBytes(byte[]? encodedValue, int offset, int count)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static string? UrlEncode(string? value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static byte[]? UrlEncodeToBytes(byte[]? value, int offset, int count)
	{
		throw null;
	}
}
