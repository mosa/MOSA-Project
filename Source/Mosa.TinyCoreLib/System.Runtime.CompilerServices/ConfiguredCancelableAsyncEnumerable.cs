using System.Threading;

namespace System.Runtime.CompilerServices;

public readonly struct ConfiguredCancelableAsyncEnumerable<T>
{
	public readonly struct Enumerator
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public T Current
		{
			get
			{
				throw null;
			}
		}

		public ConfiguredValueTaskAwaitable DisposeAsync()
		{
			throw null;
		}

		public ConfiguredValueTaskAwaitable<bool> MoveNextAsync()
		{
			throw null;
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(bool continueOnCapturedContext)
	{
		throw null;
	}

	public Enumerator GetAsyncEnumerator()
	{
		throw null;
	}

	public ConfiguredCancelableAsyncEnumerable<T> WithCancellation(CancellationToken cancellationToken)
	{
		throw null;
	}
}
