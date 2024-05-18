namespace System.Security.Policy;

public class TrustManagerContext
{
	public virtual bool IgnorePersistedDecision
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool KeepAlive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool NoPrompt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool Persist
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual ApplicationIdentity PreviousApplicationIdentity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual TrustManagerUIContext UIContext
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TrustManagerContext()
	{
	}

	public TrustManagerContext(TrustManagerUIContext uiContext)
	{
	}
}
