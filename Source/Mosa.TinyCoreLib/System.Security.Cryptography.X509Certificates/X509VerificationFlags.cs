namespace System.Security.Cryptography.X509Certificates;

[Flags]
public enum X509VerificationFlags
{
	NoFlag = 0,
	IgnoreNotTimeValid = 1,
	IgnoreCtlNotTimeValid = 2,
	IgnoreNotTimeNested = 4,
	IgnoreInvalidBasicConstraints = 8,
	AllowUnknownCertificateAuthority = 0x10,
	IgnoreWrongUsage = 0x20,
	IgnoreInvalidName = 0x40,
	IgnoreInvalidPolicy = 0x80,
	IgnoreEndRevocationUnknown = 0x100,
	IgnoreCtlSignerRevocationUnknown = 0x200,
	IgnoreCertificateAuthorityRevocationUnknown = 0x400,
	IgnoreRootRevocationUnknown = 0x800,
	AllFlags = 0xFFF
}
