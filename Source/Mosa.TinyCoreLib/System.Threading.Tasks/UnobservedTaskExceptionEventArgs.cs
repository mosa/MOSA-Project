namespace System.Threading.Tasks;

public class UnobservedTaskExceptionEventArgs : EventArgs
{
	public AggregateException Exception
	{
		get
		{
			throw null;
		}
	}

	public bool Observed
	{
		get
		{
			throw null;
		}
	}

	public UnobservedTaskExceptionEventArgs(AggregateException exception)
	{
	}

	public void SetObserved()
	{
	}
}
