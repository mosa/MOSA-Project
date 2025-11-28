using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public class CryptoConfig
{
	public static bool AllowOnlyFipsAlgorithms
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("browser")]
	public static void AddAlgorithm(Type algorithm, params string[] names)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public static void AddOID(string oid, params string[] names)
	{
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	public static object? CreateFromName(string name)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	public static object? CreateFromName(string name, params object?[]? args)
	{
		throw null;
	}

	[Obsolete("EncodeOID is obsolete. Use the ASN.1 functionality provided in System.Formats.Asn1.", DiagnosticId = "SYSLIB0031", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[UnsupportedOSPlatform("browser")]
	public static byte[] EncodeOID(string str)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static string? MapNameToOID(string name)
	{
		throw null;
	}
}
