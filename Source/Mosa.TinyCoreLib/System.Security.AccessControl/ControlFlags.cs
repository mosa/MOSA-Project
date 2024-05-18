namespace System.Security.AccessControl;

[Flags]
public enum ControlFlags
{
	None = 0,
	OwnerDefaulted = 1,
	GroupDefaulted = 2,
	DiscretionaryAclPresent = 4,
	DiscretionaryAclDefaulted = 8,
	SystemAclPresent = 0x10,
	SystemAclDefaulted = 0x20,
	DiscretionaryAclUntrusted = 0x40,
	ServerSecurity = 0x80,
	DiscretionaryAclAutoInheritRequired = 0x100,
	SystemAclAutoInheritRequired = 0x200,
	DiscretionaryAclAutoInherited = 0x400,
	SystemAclAutoInherited = 0x800,
	DiscretionaryAclProtected = 0x1000,
	SystemAclProtected = 0x2000,
	RMControlValid = 0x4000,
	SelfRelative = 0x8000
}
