namespace System.Diagnostics;

public sealed class InitializingTraceSourceEventArgs : EventArgs
{
	public TraceSource TraceSource
	{
		get
		{
			throw null;
		}
	}

	public bool WasInitialized
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public InitializingTraceSourceEventArgs(TraceSource traceSource)
	{
	}
}
