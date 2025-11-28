namespace System.DirectoryServices.Protocols;

public class SecurityDescriptorFlagControl : DirectoryControl
{
	public SecurityMasks SecurityMasks
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityDescriptorFlagControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public SecurityDescriptorFlagControl(SecurityMasks masks)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
