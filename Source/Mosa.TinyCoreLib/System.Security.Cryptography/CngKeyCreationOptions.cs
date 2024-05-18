namespace System.Security.Cryptography;

[Flags]
public enum CngKeyCreationOptions
{
	None = 0,
	MachineKey = 0x20,
	OverwriteExistingKey = 0x80
}
