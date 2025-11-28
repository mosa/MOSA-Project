namespace System.DirectoryServices.AccountManagement;

[Flags]
public enum ContextOptions
{
	Negotiate = 1,
	SimpleBind = 2,
	SecureSocketLayer = 4,
	Signing = 8,
	Sealing = 0x10,
	ServerBind = 0x20
}
