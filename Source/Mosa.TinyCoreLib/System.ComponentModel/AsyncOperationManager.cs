using System.Threading;

namespace System.ComponentModel;

public static class AsyncOperationManager
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static SynchronizationContext SynchronizationContext
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static AsyncOperation CreateOperation(object? userSuppliedState)
	{
		throw null;
	}
}
