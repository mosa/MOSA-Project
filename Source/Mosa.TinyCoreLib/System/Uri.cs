using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System;

public class Uri : ISpanFormattable, IFormattable, ISerializable
{
	public static readonly string SchemeDelimiter;

	public static readonly string UriSchemeFile;

	public static readonly string UriSchemeFtp;

	public static readonly string UriSchemeFtps;

	public static readonly string UriSchemeGopher;

	public static readonly string UriSchemeHttp;

	public static readonly string UriSchemeHttps;

	public static readonly string UriSchemeMailto;

	public static readonly string UriSchemeNetPipe;

	public static readonly string UriSchemeNetTcp;

	public static readonly string UriSchemeNews;

	public static readonly string UriSchemeNntp;

	public static readonly string UriSchemeSftp;

	public static readonly string UriSchemeSsh;

	public static readonly string UriSchemeTelnet;

	public static readonly string UriSchemeWs;

	public static readonly string UriSchemeWss;

	public string AbsolutePath
	{
		get
		{
			throw null;
		}
	}

	public string AbsoluteUri
	{
		get
		{
			throw null;
		}
	}

	public string Authority
	{
		get
		{
			throw null;
		}
	}

	public string DnsSafeHost
	{
		get
		{
			throw null;
		}
	}

	public string Fragment
	{
		get
		{
			throw null;
		}
	}

	public string Host
	{
		get
		{
			throw null;
		}
	}

	public UriHostNameType HostNameType
	{
		get
		{
			throw null;
		}
	}

	public string IdnHost
	{
		get
		{
			throw null;
		}
	}

	public bool IsAbsoluteUri
	{
		get
		{
			throw null;
		}
	}

	public bool IsDefaultPort
	{
		get
		{
			throw null;
		}
	}

	public bool IsFile
	{
		get
		{
			throw null;
		}
	}

	public bool IsLoopback
	{
		get
		{
			throw null;
		}
	}

	public bool IsUnc
	{
		get
		{
			throw null;
		}
	}

	public string LocalPath
	{
		get
		{
			throw null;
		}
	}

	public string OriginalString
	{
		get
		{
			throw null;
		}
	}

	public string PathAndQuery
	{
		get
		{
			throw null;
		}
	}

	public int Port
	{
		get
		{
			throw null;
		}
	}

	public string Query
	{
		get
		{
			throw null;
		}
	}

	public string Scheme
	{
		get
		{
			throw null;
		}
	}

	public string[] Segments
	{
		get
		{
			throw null;
		}
	}

	public bool UserEscaped
	{
		get
		{
			throw null;
		}
	}

	public string UserInfo
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Uri(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public Uri([StringSyntax("Uri")] string uriString)
	{
	}

	[Obsolete("This constructor has been deprecated; the dontEscape parameter is always false. Use Uri(string) instead.")]
	public Uri([StringSyntax("Uri")] string uriString, bool dontEscape)
	{
	}

	public Uri([StringSyntax("Uri")] string uriString, in UriCreationOptions creationOptions)
	{
	}

	public Uri([StringSyntax("Uri", new object[] { "uriKind" })] string uriString, UriKind uriKind)
	{
	}

	public Uri(Uri baseUri, string? relativeUri)
	{
	}

	[Obsolete("This constructor has been deprecated; the dontEscape parameter is always false. Use Uri(Uri, string) instead.")]
	public Uri(Uri baseUri, string? relativeUri, bool dontEscape)
	{
	}

	public Uri(Uri baseUri, Uri relativeUri)
	{
	}

	[Obsolete("Uri.Canonicalize has been deprecated and is not supported.")]
	protected virtual void Canonicalize()
	{
	}

	public static UriHostNameType CheckHostName(string? name)
	{
		throw null;
	}

	public static bool CheckSchemeName([NotNullWhen(true)] string? schemeName)
	{
		throw null;
	}

	[Obsolete("Uri.CheckSecurity has been deprecated and is not supported.")]
	protected virtual void CheckSecurity()
	{
	}

	public static int Compare(Uri? uri1, Uri? uri2, UriComponents partsToCompare, UriFormat compareFormat, StringComparison comparisonType)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? comparand)
	{
		throw null;
	}

	[Obsolete("Uri.Escape has been deprecated and is not supported.")]
	protected virtual void Escape()
	{
	}

	public static string EscapeDataString(string stringToEscape)
	{
		throw null;
	}

	[Obsolete("Uri.EscapeString has been deprecated. Use GetComponents() or Uri.EscapeDataString to escape a Uri component or a string.")]
	protected static string EscapeString(string? str)
	{
		throw null;
	}

	[Obsolete("Uri.EscapeUriString can corrupt the Uri string in some cases. Consider using Uri.EscapeDataString for query string components instead.", DiagnosticId = "SYSLIB0013", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static string EscapeUriString(string stringToEscape)
	{
		throw null;
	}

	public static int FromHex(char digit)
	{
		throw null;
	}

	public string GetComponents(UriComponents components, UriFormat format)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public string GetLeftPart(UriPartial part)
	{
		throw null;
	}

	protected void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public static string HexEscape(char character)
	{
		throw null;
	}

	public static char HexUnescape(string pattern, ref int index)
	{
		throw null;
	}

	[Obsolete("Uri.IsBadFileSystemCharacter has been deprecated and is not supported.")]
	protected virtual bool IsBadFileSystemCharacter(char character)
	{
		throw null;
	}

	public bool IsBaseOf(Uri uri)
	{
		throw null;
	}

	[Obsolete("Uri.IsExcludedCharacter has been deprecated and is not supported.")]
	protected static bool IsExcludedCharacter(char character)
	{
		throw null;
	}

	public static bool IsHexDigit(char character)
	{
		throw null;
	}

	public static bool IsHexEncoding(string pattern, int index)
	{
		throw null;
	}

	[Obsolete("Uri.IsReservedCharacter has been deprecated and is not supported.")]
	protected virtual bool IsReservedCharacter(char character)
	{
		throw null;
	}

	public bool IsWellFormedOriginalString()
	{
		throw null;
	}

	public static bool IsWellFormedUriString([NotNullWhen(true)][StringSyntax("Uri", new object[] { "uriKind" })] string? uriString, UriKind uriKind)
	{
		throw null;
	}

	[Obsolete("Uri.MakeRelative has been deprecated. Use MakeRelativeUri(Uri uri) instead.")]
	public string MakeRelative(Uri toUri)
	{
		throw null;
	}

	public Uri MakeRelativeUri(Uri uri)
	{
		throw null;
	}

	public static bool operator ==(Uri? uri1, Uri? uri2)
	{
		throw null;
	}

	public static bool operator !=(Uri? uri1, Uri? uri2)
	{
		throw null;
	}

	[Obsolete("Uri.Parse has been deprecated and is not supported.")]
	protected virtual void Parse()
	{
	}

	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public static bool TryCreate([NotNullWhen(true)][StringSyntax("Uri")] string? uriString, in UriCreationOptions creationOptions, [NotNullWhen(true)] out Uri? result)
	{
		throw null;
	}

	public static bool TryCreate([NotNullWhen(true)][StringSyntax("Uri", new object[] { "uriKind" })] string? uriString, UriKind uriKind, [NotNullWhen(true)] out Uri? result)
	{
		throw null;
	}

	public static bool TryCreate(Uri? baseUri, string? relativeUri, [NotNullWhen(true)] out Uri? result)
	{
		throw null;
	}

	public static bool TryCreate(Uri? baseUri, Uri? relativeUri, [NotNullWhen(true)] out Uri? result)
	{
		throw null;
	}

	public bool TryFormat(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	[Obsolete("Uri.Unescape has been deprecated. Use GetComponents() or Uri.UnescapeDataString() to unescape a Uri component or a string.")]
	protected virtual string Unescape(string path)
	{
		throw null;
	}

	public static string UnescapeDataString(string stringToUnescape)
	{
		throw null;
	}
}
