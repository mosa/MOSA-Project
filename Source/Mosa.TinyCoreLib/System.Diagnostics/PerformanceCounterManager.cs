namespace System.Diagnostics;

public sealed class PerformanceCounterManager : ICollectData
{
	[Obsolete("PerformanceCounterManager has been deprecated. Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.")]
	public PerformanceCounterManager()
	{
	}

	[Obsolete("PerformanceCounterManager has been deprecated. Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.")]
	void ICollectData.CloseData()
	{
	}

	[Obsolete("PerformanceCounterManager has been deprecated. Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.")]
	void ICollectData.CollectData(int callIdx, IntPtr valueNamePtr, IntPtr dataPtr, int totalBytes, out IntPtr res)
	{
		throw null;
	}
}
