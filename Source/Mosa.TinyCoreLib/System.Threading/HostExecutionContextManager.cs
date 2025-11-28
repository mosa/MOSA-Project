namespace System.Threading;

public class HostExecutionContextManager
{
	public virtual HostExecutionContext? Capture()
	{
		throw null;
	}

	public virtual void Revert(object previousState)
	{
	}

	public virtual object SetHostExecutionContext(HostExecutionContext hostExecutionContext)
	{
		throw null;
	}
}
