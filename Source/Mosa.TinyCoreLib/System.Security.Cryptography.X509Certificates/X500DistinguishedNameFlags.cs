namespace System.Security.Cryptography.X509Certificates;

[Flags]
public enum X500DistinguishedNameFlags
{
	None = 0,
	Reversed = 1,
	UseSemicolons = 0x10,
	DoNotUsePlusSign = 0x20,
	DoNotUseQuotes = 0x40,
	UseCommas = 0x80,
	UseNewLines = 0x100,
	UseUTF8Encoding = 0x1000,
	UseT61Encoding = 0x2000,
	ForceUTF8Encoding = 0x4000
}
