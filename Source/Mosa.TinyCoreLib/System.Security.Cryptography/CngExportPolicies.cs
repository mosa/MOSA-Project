namespace System.Security.Cryptography;

[Flags]
public enum CngExportPolicies
{
	None = 0,
	AllowExport = 1,
	AllowPlaintextExport = 2,
	AllowArchiving = 4,
	AllowPlaintextArchiving = 8
}
