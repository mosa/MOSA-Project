namespace System.DirectoryServices.Protocols;

public class DomainScopeControl : DirectoryControl
{
	public DomainScopeControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
