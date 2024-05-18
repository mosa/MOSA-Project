using System.Runtime.Versioning;
using System.Threading;

namespace System.Transactions;

[UnsupportedOSPlatform("browser")]
public sealed class CommittableTransaction : Transaction, IAsyncResult
{
	object? IAsyncResult.AsyncState
	{
		get
		{
			throw null;
		}
	}

	WaitHandle IAsyncResult.AsyncWaitHandle
	{
		get
		{
			throw null;
		}
	}

	bool IAsyncResult.CompletedSynchronously
	{
		get
		{
			throw null;
		}
	}

	bool IAsyncResult.IsCompleted
	{
		get
		{
			throw null;
		}
	}

	public CommittableTransaction()
	{
	}

	public CommittableTransaction(TimeSpan timeout)
	{
	}

	public CommittableTransaction(TransactionOptions options)
	{
	}

	public IAsyncResult BeginCommit(AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public void Commit()
	{
	}

	public void EndCommit(IAsyncResult asyncResult)
	{
	}
}
