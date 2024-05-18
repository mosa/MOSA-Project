namespace System.Threading;

public struct NativeOverlapped
{
	public IntPtr EventHandle;

	public IntPtr InternalHigh;

	public IntPtr InternalLow;

	public int OffsetHigh;

	public int OffsetLow;
}
