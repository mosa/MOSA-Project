using System.Collections.Generic;

namespace System;

public static class GC
{
	public static int MaxGeneration
	{
		get
		{
			throw null;
		}
	}

	public static void AddMemoryPressure(long bytesAllocated)
	{
	}

	public static T[] AllocateArray<T>(int length, bool pinned = false)
	{
		throw null;
	}

	public static T[] AllocateUninitializedArray<T>(int length, bool pinned = false)
	{
		throw null;
	}

	public static void CancelFullGCNotification()
	{
	}

	public static void Collect()
	{
	}

	public static void Collect(int generation)
	{
	}

	public static void Collect(int generation, GCCollectionMode mode)
	{
	}

	public static void Collect(int generation, GCCollectionMode mode, bool blocking)
	{
	}

	public static void Collect(int generation, GCCollectionMode mode, bool blocking, bool compacting)
	{
	}

	public static int CollectionCount(int generation)
	{
		throw null;
	}

	public static void EndNoGCRegion()
	{
	}

	public static long GetAllocatedBytesForCurrentThread()
	{
		throw null;
	}

	public static GCMemoryInfo GetGCMemoryInfo()
	{
		throw null;
	}

	public static GCMemoryInfo GetGCMemoryInfo(GCKind kind)
	{
		throw null;
	}

	public static int GetGeneration(object obj)
	{
		throw null;
	}

	public static int GetGeneration(WeakReference wo)
	{
		throw null;
	}

	public static long GetTotalAllocatedBytes(bool precise = false)
	{
		throw null;
	}

	public static long GetTotalMemory(bool forceFullCollection)
	{
		throw null;
	}

	public static void KeepAlive(object? obj)
	{
	}

	public static void RegisterForFullGCNotification(int maxGenerationThreshold, int largeObjectHeapThreshold)
	{
	}

	public static void RegisterNoGCRegionCallback(long totalSize, Action callback)
	{
	}

	public static void RemoveMemoryPressure(long bytesAllocated)
	{
	}

	public static void ReRegisterForFinalize(object obj)
	{
	}

	public static void SuppressFinalize(object obj)
	{
	}

	public static bool TryStartNoGCRegion(long totalSize)
	{
		throw null;
	}

	public static bool TryStartNoGCRegion(long totalSize, bool disallowFullBlockingGC)
	{
		throw null;
	}

	public static bool TryStartNoGCRegion(long totalSize, long lohSize)
	{
		throw null;
	}

	public static bool TryStartNoGCRegion(long totalSize, long lohSize, bool disallowFullBlockingGC)
	{
		throw null;
	}

	public static GCNotificationStatus WaitForFullGCApproach()
	{
		throw null;
	}

	public static GCNotificationStatus WaitForFullGCApproach(int millisecondsTimeout)
	{
		throw null;
	}

	public static GCNotificationStatus WaitForFullGCApproach(TimeSpan timeout)
	{
		throw null;
	}

	public static GCNotificationStatus WaitForFullGCComplete()
	{
		throw null;
	}

	public static GCNotificationStatus WaitForFullGCComplete(int millisecondsTimeout)
	{
		throw null;
	}

	public static GCNotificationStatus WaitForFullGCComplete(TimeSpan timeout)
	{
		throw null;
	}

	public static void WaitForPendingFinalizers()
	{
	}

	public static TimeSpan GetTotalPauseDuration()
	{
		throw null;
	}

	public static IReadOnlyDictionary<string, object> GetConfigurationVariables()
	{
		throw null;
	}

	public static void RefreshMemoryLimit()
	{
		throw null;
	}
}
