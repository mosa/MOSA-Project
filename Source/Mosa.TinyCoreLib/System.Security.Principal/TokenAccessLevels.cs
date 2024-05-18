namespace System.Security.Principal;

[Flags]
public enum TokenAccessLevels
{
	AssignPrimary = 1,
	Duplicate = 2,
	Impersonate = 4,
	Query = 8,
	QuerySource = 0x10,
	AdjustPrivileges = 0x20,
	AdjustGroups = 0x40,
	AdjustDefault = 0x80,
	AdjustSessionId = 0x100,
	Read = 0x20008,
	Write = 0x200E0,
	AllAccess = 0xF01FF,
	MaximumAllowed = 0x2000000
}
