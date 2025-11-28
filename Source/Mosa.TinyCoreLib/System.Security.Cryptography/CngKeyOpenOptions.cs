namespace System.Security.Cryptography;

[Flags]
public enum CngKeyOpenOptions
{
	None = 0,
	UserKey = 0,
	MachineKey = 0x20,
	Silent = 0x40
}
