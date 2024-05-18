using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;

namespace System.Threading.Tasks;

[AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder))]
public readonly struct ValueTask : IEquatable<ValueTask>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static ValueTask CompletedTask
	{
		get
		{
			throw null;
		}
	}

	public bool IsCanceled
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompletedSuccessfully
	{
		get
		{
			throw null;
		}
	}

	public bool IsFaulted
	{
		get
		{
			throw null;
		}
	}

	public ValueTask(IValueTaskSource source, short token)
	{
		throw null;
	}

	public ValueTask(Task task)
	{
		throw null;
	}

	public Task AsTask()
	{
		throw null;
	}

	public ConfiguredValueTaskAwaitable ConfigureAwait(bool continueOnCapturedContext)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ValueTask other)
	{
		throw null;
	}

	public static ValueTask FromCanceled(CancellationToken cancellationToken)
	{
		throw null;
	}

	public static ValueTask<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
	{
		throw null;
	}

	public static ValueTask FromException(Exception exception)
	{
		throw null;
	}

	public static ValueTask<TResult> FromException<TResult>(Exception exception)
	{
		throw null;
	}

	public static ValueTask<TResult> FromResult<TResult>(TResult result)
	{
		throw null;
	}

	public ValueTaskAwaiter GetAwaiter()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ValueTask left, ValueTask right)
	{
		throw null;
	}

	public static bool operator !=(ValueTask left, ValueTask right)
	{
		throw null;
	}

	public ValueTask Preserve()
	{
		throw null;
	}
}
[AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder<>))]
public readonly struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
{
	private readonly TResult _result;

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public bool IsCanceled
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompletedSuccessfully
	{
		get
		{
			throw null;
		}
	}

	public bool IsFaulted
	{
		get
		{
			throw null;
		}
	}

	public TResult Result
	{
		get
		{
			throw null;
		}
	}

	public ValueTask(IValueTaskSource<TResult> source, short token)
	{
		throw null;
	}

	public ValueTask(Task<TResult> task)
	{
		throw null;
	}

	public ValueTask(TResult result)
	{
		throw null;
	}

	public Task<TResult> AsTask()
	{
		throw null;
	}

	public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ValueTask<TResult> other)
	{
		throw null;
	}

	public ValueTaskAwaiter<TResult> GetAwaiter()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right)
	{
		throw null;
	}

	public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right)
	{
		throw null;
	}

	public ValueTask<TResult> Preserve()
	{
		throw null;
	}

	public override string? ToString()
	{
		throw null;
	}
}
