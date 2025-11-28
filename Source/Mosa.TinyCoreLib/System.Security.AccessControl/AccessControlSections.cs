namespace System.Security.AccessControl;

[Flags]
public enum AccessControlSections
{
	None = 0,
	Audit = 1,
	Access = 2,
	Owner = 4,
	Group = 8,
	All = 0xF
}
