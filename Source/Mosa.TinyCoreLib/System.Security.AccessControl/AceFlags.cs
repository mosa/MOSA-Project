namespace System.Security.AccessControl;

[Flags]
public enum AceFlags : byte
{
	None = 0,
	ObjectInherit = 1,
	ContainerInherit = 2,
	NoPropagateInherit = 4,
	InheritOnly = 8,
	InheritanceFlags = 0xF,
	Inherited = 0x10,
	SuccessfulAccess = 0x40,
	FailedAccess = 0x80,
	AuditFlags = 0xC0
}
