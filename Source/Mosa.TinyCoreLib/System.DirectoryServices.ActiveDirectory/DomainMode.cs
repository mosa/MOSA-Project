namespace System.DirectoryServices.ActiveDirectory;

public enum DomainMode
{
	Unknown = -1,
	Windows2000MixedDomain,
	Windows2000NativeDomain,
	Windows2003InterimDomain,
	Windows2003Domain,
	Windows2008Domain,
	Windows2008R2Domain,
	Windows8Domain,
	Windows2012R2Domain
}
