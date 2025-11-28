namespace System.Security.Cryptography.X509Certificates;

[Flags]
public enum X509KeyUsageFlags
{
	None = 0,
	EncipherOnly = 1,
	CrlSign = 2,
	KeyCertSign = 4,
	KeyAgreement = 8,
	DataEncipherment = 0x10,
	KeyEncipherment = 0x20,
	NonRepudiation = 0x40,
	DigitalSignature = 0x80,
	DecipherOnly = 0x8000
}
