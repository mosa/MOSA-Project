namespace System.ComponentModel;

public class BackgroundWorker : Component
{
	public bool CancellationPending
	{
		get
		{
			throw null;
		}
	}

	public bool IsBusy
	{
		get
		{
			throw null;
		}
	}

	public bool WorkerReportsProgress
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool WorkerSupportsCancellation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event DoWorkEventHandler? DoWork
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ProgressChangedEventHandler? ProgressChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event RunWorkerCompletedEventHandler? RunWorkerCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public void CancelAsync()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected virtual void OnDoWork(DoWorkEventArgs e)
	{
	}

	protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
	{
	}

	protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
	{
	}

	public void ReportProgress(int percentProgress)
	{
	}

	public void ReportProgress(int percentProgress, object? userState)
	{
	}

	public void RunWorkerAsync()
	{
	}

	public void RunWorkerAsync(object? argument)
	{
	}
}
