namespace System.Threading.Tasks;

public class ParallelLoopState
{
	public bool IsExceptional
	{
		get
		{
			throw null;
		}
	}

	public bool IsStopped
	{
		get
		{
			throw null;
		}
	}

	public long? LowestBreakIteration
	{
		get
		{
			throw null;
		}
	}

	public bool ShouldExitCurrentIteration
	{
		get
		{
			throw null;
		}
	}

	internal ParallelLoopState()
	{
	}

	public void Break()
	{
	}

	public void Stop()
	{
	}
}
