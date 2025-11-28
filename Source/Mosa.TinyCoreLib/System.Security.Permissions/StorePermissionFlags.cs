namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[Flags]
public enum StorePermissionFlags
{
	NoFlags = 0,
	CreateStore = 1,
	DeleteStore = 2,
	EnumerateStores = 4,
	OpenStore = 0x10,
	AddToStore = 0x20,
	RemoveFromStore = 0x40,
	EnumerateCertificates = 0x80,
	AllFlags = 0xF7
}
