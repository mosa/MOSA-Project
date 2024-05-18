namespace System.Runtime.CompilerServices;

public readonly struct ConfiguredTaskAwaitable
{
	public readonly struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public bool IsCompleted
		{
			get
			{
				throw null;
			}
		}

		public void GetResult()
		{
		}

		public void OnCompleted(Action continuation)
		{
		}

		public void UnsafeOnCompleted(Action continuation)
		{
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ConfiguredTaskAwaiter GetAwaiter()
	{
		throw null;
	}
}
public readonly struct ConfiguredTaskAwaitable<TResult>
{
	public readonly struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public bool IsCompleted
		{
			get
			{
				throw null;
			}
		}

		public TResult GetResult()
		{
			throw null;
		}

		public void OnCompleted(Action continuation)
		{
		}

		public void UnsafeOnCompleted(Action continuation)
		{
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ConfiguredTaskAwaiter GetAwaiter()
	{
		throw null;
	}
}
