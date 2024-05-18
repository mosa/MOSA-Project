namespace System.Security.Cryptography;

[Flags]
public enum CngUIProtectionLevels
{
	None = 0,
	ProtectKey = 1,
	ForceHighProtection = 2
}
