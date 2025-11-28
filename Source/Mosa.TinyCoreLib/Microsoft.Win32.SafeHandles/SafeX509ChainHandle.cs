namespace Microsoft.Win32.SafeHandles;

public sealed class SafeX509ChainHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeX509ChainHandle()
		: base(ownsHandle: false)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
