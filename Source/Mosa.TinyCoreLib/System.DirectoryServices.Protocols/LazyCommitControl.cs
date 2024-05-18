namespace System.DirectoryServices.Protocols;

public class LazyCommitControl : DirectoryControl
{
	public LazyCommitControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}
}
