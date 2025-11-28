namespace System.Diagnostics;

[Flags]
public enum EventLogPermissionAccess
{
	None = 0,
	Browse = 2,
	Instrument = 6,
	Audit = 0xA,
	Write = 0x10,
	Administer = 0x30
}
