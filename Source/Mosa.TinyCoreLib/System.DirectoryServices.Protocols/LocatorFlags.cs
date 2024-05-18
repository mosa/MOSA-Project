namespace System.DirectoryServices.Protocols;

[Flags]
public enum LocatorFlags : long
{
	None = 0L,
	ForceRediscovery = 1L,
	DirectoryServicesRequired = 0x10L,
	DirectoryServicesPreferred = 0x20L,
	GCRequired = 0x40L,
	PdcRequired = 0x80L,
	IPRequired = 0x200L,
	KdcRequired = 0x400L,
	TimeServerRequired = 0x800L,
	WriteableRequired = 0x1000L,
	GoodTimeServerPreferred = 0x2000L,
	AvoidSelf = 0x4000L,
	OnlyLdapNeeded = 0x8000L,
	IsFlatName = 0x10000L,
	IsDnsName = 0x20000L,
	ReturnDnsName = 0x40000000L,
	ReturnFlatName = 0x80000000L
}
