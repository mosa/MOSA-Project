namespace System.ComponentModel.Design;

public abstract class DesignerTransaction : IDisposable
{
	public bool Canceled
	{
		get
		{
			throw null;
		}
	}

	public bool Committed
	{
		get
		{
			throw null;
		}
	}

	public string Description
	{
		get
		{
			throw null;
		}
	}

	protected DesignerTransaction()
	{
	}

	protected DesignerTransaction(string description)
	{
	}

	public void Cancel()
	{
	}

	public void Commit()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~DesignerTransaction()
	{
	}

	protected abstract void OnCancel();

	protected abstract void OnCommit();

	void IDisposable.Dispose()
	{
	}
}
