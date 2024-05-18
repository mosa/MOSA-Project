namespace System.Threading.Tasks.Sources;

public struct ManualResetValueTaskSourceCore<TResult>
{
	private TResult _result;

	private object _dummy;

	private int _dummyPrimitive;

	public bool RunContinuationsAsynchronously
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public short Version
	{
		get
		{
			throw null;
		}
	}

	public TResult GetResult(short token)
	{
		throw null;
	}

	public ValueTaskSourceStatus GetStatus(short token)
	{
		throw null;
	}

	public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
	{
	}

	public void Reset()
	{
	}

	public void SetException(Exception error)
	{
	}

	public void SetResult(TResult result)
	{
	}
}
