namespace System.DirectoryServices.Protocols;

public class CrossDomainMoveControl : DirectoryControl
{
	public string TargetDomainController
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CrossDomainMoveControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public CrossDomainMoveControl(string targetDomainController)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
