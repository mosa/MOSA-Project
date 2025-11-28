namespace System.Security.Authentication;

[Flags]
public enum SslProtocols
{
	None = 0,
	[Obsolete("SslProtocols.Ssl2 has been deprecated and is not supported.")]
	Ssl2 = 0xC,
	[Obsolete("SslProtocols.Ssl3 has been deprecated and is not supported.")]
	Ssl3 = 0x30,
	[Obsolete("TLS versions 1.0 and 1.1 have known vulnerabilities and are not recommended. Use a newer TLS version instead, or use SslProtocols.None to defer to OS defaults.", DiagnosticId = "SYSLIB0039", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	Tls = 0xC0,
	[Obsolete("SslProtocols.Default has been deprecated and is not supported.")]
	Default = 0xF0,
	[Obsolete("TLS versions 1.0 and 1.1 have known vulnerabilities and are not recommended. Use a newer TLS version instead, or use SslProtocols.None to defer to OS defaults.", DiagnosticId = "SYSLIB0039", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	Tls11 = 0x300,
	Tls12 = 0xC00,
	Tls13 = 0x3000
}
