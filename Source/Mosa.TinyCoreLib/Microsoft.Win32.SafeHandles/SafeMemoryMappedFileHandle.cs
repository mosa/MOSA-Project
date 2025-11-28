namespace Microsoft.Win32.SafeHandles;

public sealed class SafeMemoryMappedFileHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	public SafeMemoryMappedFileHandle()
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
