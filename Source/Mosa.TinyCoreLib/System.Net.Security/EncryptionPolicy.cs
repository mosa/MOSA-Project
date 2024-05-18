namespace System.Net.Security;

public enum EncryptionPolicy
{
	RequireEncryption,
	[Obsolete("EncryptionPolicy.NoEncryption and AllowEncryption significantly reduce security and should not be used in production code.", DiagnosticId = "SYSLIB0040", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	AllowNoEncryption,
	[Obsolete("EncryptionPolicy.NoEncryption and AllowEncryption significantly reduce security and should not be used in production code.", DiagnosticId = "SYSLIB0040", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	NoEncryption
}
