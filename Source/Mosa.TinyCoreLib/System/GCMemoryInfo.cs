namespace System;

public readonly struct GCMemoryInfo
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public bool Compacted
	{
		get
		{
			throw null;
		}
	}

	public bool Concurrent
	{
		get
		{
			throw null;
		}
	}

	public long FinalizationPendingCount
	{
		get
		{
			throw null;
		}
	}

	public long FragmentedBytes
	{
		get
		{
			throw null;
		}
	}

	public int Generation
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySpan<GCGenerationInfo> GenerationInfo
	{
		get
		{
			throw null;
		}
	}

	public long HeapSizeBytes
	{
		get
		{
			throw null;
		}
	}

	public long HighMemoryLoadThresholdBytes
	{
		get
		{
			throw null;
		}
	}

	public long Index
	{
		get
		{
			throw null;
		}
	}

	public long MemoryLoadBytes
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySpan<TimeSpan> PauseDurations
	{
		get
		{
			throw null;
		}
	}

	public double PauseTimePercentage
	{
		get
		{
			throw null;
		}
	}

	public long PinnedObjectsCount
	{
		get
		{
			throw null;
		}
	}

	public long PromotedBytes
	{
		get
		{
			throw null;
		}
	}

	public long TotalAvailableMemoryBytes
	{
		get
		{
			throw null;
		}
	}

	public long TotalCommittedBytes
	{
		get
		{
			throw null;
		}
	}
}
