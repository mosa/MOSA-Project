namespace System.Diagnostics;

public class Stopwatch
{
	public static readonly long Frequency;

	public static readonly bool IsHighResolution;

	public TimeSpan Elapsed
	{
		get
		{
			throw null;
		}
	}

	public long ElapsedMilliseconds
	{
		get
		{
			throw null;
		}
	}

	public long ElapsedTicks
	{
		get
		{
			throw null;
		}
	}

	public bool IsRunning
	{
		get
		{
			throw null;
		}
	}

	public static long GetTimestamp()
	{
		throw null;
	}

	public static TimeSpan GetElapsedTime(long startingTimestamp)
	{
		throw null;
	}

	public static TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp)
	{
		throw null;
	}

	public void Reset()
	{
	}

	public void Restart()
	{
	}

	public void Start()
	{
	}

	public static Stopwatch StartNew()
	{
		throw null;
	}

	public void Stop()
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
