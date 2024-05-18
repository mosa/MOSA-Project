using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks.Dataflow;

public static class DataflowBlock
{
	public static IAsyncEnumerable<TOutput> ReceiveAllAsync<TOutput>(this IReceivableSourceBlock<TOutput> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static IObservable<TOutput> AsObservable<TOutput>(this ISourceBlock<TOutput> source)
	{
		throw null;
	}

	public static IObserver<TInput> AsObserver<TInput>(this ITargetBlock<TInput> target)
	{
		throw null;
	}

	public static Task<int> Choose<T1, T2>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2)
	{
		throw null;
	}

	public static Task<int> Choose<T1, T2>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, DataflowBlockOptions dataflowBlockOptions)
	{
		throw null;
	}

	public static Task<int> Choose<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3)
	{
		throw null;
	}

	public static Task<int> Choose<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3, DataflowBlockOptions dataflowBlockOptions)
	{
		throw null;
	}

	public static IPropagatorBlock<TInput, TOutput> Encapsulate<TInput, TOutput>(ITargetBlock<TInput> target, ISourceBlock<TOutput> source)
	{
		throw null;
	}

	public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target)
	{
		throw null;
	}

	public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target, Predicate<TOutput> predicate)
	{
		throw null;
	}

	public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions, Predicate<TOutput> predicate)
	{
		throw null;
	}

	public static ITargetBlock<TInput> NullTarget<TInput>()
	{
		throw null;
	}

	public static Task<bool> OutputAvailableAsync<TOutput>(this ISourceBlock<TOutput> source)
	{
		throw null;
	}

	public static Task<bool> OutputAvailableAsync<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static bool Post<TInput>(this ITargetBlock<TInput> target, TInput item)
	{
		throw null;
	}

	public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source)
	{
		throw null;
	}

	public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout)
	{
		throw null;
	}

	public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source)
	{
		throw null;
	}

	public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout)
	{
		throw null;
	}

	public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static Task<bool> SendAsync<TInput>(this ITargetBlock<TInput> target, TInput item)
	{
		throw null;
	}

	public static Task<bool> SendAsync<TInput>(this ITargetBlock<TInput> target, TInput item, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static bool TryReceive<TOutput>(this IReceivableSourceBlock<TOutput> source, [MaybeNullWhen(false)] out TOutput item)
	{
		throw null;
	}
}
