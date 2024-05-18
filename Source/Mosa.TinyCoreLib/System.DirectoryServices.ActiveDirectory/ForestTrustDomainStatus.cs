namespace System.DirectoryServices.ActiveDirectory;

public enum ForestTrustDomainStatus
{
	Enabled = 0,
	SidAdminDisabled = 1,
	SidConflictDisabled = 2,
	NetBiosNameAdminDisabled = 4,
	NetBiosNameConflictDisabled = 8
}
