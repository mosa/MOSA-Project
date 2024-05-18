namespace System.Security.Cryptography.X509Certificates;

[Flags]
public enum X509ChainStatusFlags
{
	NoError = 0,
	NotTimeValid = 1,
	NotTimeNested = 2,
	Revoked = 4,
	NotSignatureValid = 8,
	NotValidForUsage = 0x10,
	UntrustedRoot = 0x20,
	RevocationStatusUnknown = 0x40,
	Cyclic = 0x80,
	InvalidExtension = 0x100,
	InvalidPolicyConstraints = 0x200,
	InvalidBasicConstraints = 0x400,
	InvalidNameConstraints = 0x800,
	HasNotSupportedNameConstraint = 0x1000,
	HasNotDefinedNameConstraint = 0x2000,
	HasNotPermittedNameConstraint = 0x4000,
	HasExcludedNameConstraint = 0x8000,
	PartialChain = 0x10000,
	CtlNotTimeValid = 0x20000,
	CtlNotSignatureValid = 0x40000,
	CtlNotValidForUsage = 0x80000,
	HasWeakSignature = 0x100000,
	OfflineRevocation = 0x1000000,
	NoIssuanceChainPolicy = 0x2000000,
	ExplicitDistrust = 0x4000000,
	HasNotSupportedCriticalExtension = 0x8000000
}
