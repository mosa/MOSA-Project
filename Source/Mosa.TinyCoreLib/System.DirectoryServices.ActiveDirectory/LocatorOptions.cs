namespace System.DirectoryServices.ActiveDirectory;

[Flags]
public enum LocatorOptions : long
{
	ForceRediscovery = 1L,
	KdcRequired = 0x400L,
	TimeServerRequired = 0x800L,
	WriteableRequired = 0x1000L,
	AvoidSelf = 0x4000L
}
