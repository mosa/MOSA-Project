namespace System.DirectoryServices;

[Flags]
public enum AuthenticationTypes
{
	None = 0,
	Secure = 1,
	Encryption = 2,
	SecureSocketsLayer = 2,
	ReadonlyServer = 4,
	Anonymous = 0x10,
	FastBind = 0x20,
	Signing = 0x40,
	Sealing = 0x80,
	Delegation = 0x100,
	ServerBind = 0x200
}
