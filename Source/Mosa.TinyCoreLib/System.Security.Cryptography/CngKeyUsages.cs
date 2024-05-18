namespace System.Security.Cryptography;

[Flags]
public enum CngKeyUsages
{
	None = 0,
	Decryption = 1,
	Signing = 2,
	KeyAgreement = 4,
	AllUsages = 0xFFFFFF
}
