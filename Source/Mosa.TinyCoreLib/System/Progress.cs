namespace System;

public class Progress<T> : IProgress<T>
{
	public event EventHandler<T>? ProgressChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public Progress()
	{
	}

	public Progress(Action<T> handler)
	{
	}

	protected virtual void OnReport(T value)
	{
	}

	void IProgress<T>.Report(T value)
	{
	}
}
