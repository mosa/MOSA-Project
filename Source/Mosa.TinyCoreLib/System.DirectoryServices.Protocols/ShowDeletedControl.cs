namespace System.DirectoryServices.Protocols;

public class ShowDeletedControl : DirectoryControl
{
	public ShowDeletedControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
