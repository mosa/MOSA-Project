namespace System.DirectoryServices.Protocols;

[Flags]
public enum ReferralChasingOptions
{
	None = 0,
	Subordinate = 0x20,
	External = 0x40,
	All = 0x60
}
