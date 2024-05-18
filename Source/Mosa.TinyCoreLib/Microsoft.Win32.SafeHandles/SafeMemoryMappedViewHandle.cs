using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafeMemoryMappedViewHandle : SafeBuffer
{
	public SafeMemoryMappedViewHandle()
		: base(ownsHandle: false)
	{
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}
}
