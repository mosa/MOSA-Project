using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;

namespace System.Threading.Tasks;

public static class Parallel
{
	public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int> body)
	{
		throw null;
	}

	public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body)
	{
		throw null;
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long> body)
	{
		throw null;
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body)
	{
		throw null;
	}

	public static Task ForAsync<T>(T fromInclusive, T toExclusive, Func<T, CancellationToken, ValueTask> body) where T : notnull, IBinaryInteger<T>
	{
		throw null;
	}

	public static Task ForAsync<T>(T fromInclusive, T toExclusive, CancellationToken cancellationToken, Func<T, CancellationToken, ValueTask> body) where T : notnull, IBinaryInteger<T>
	{
		throw null;
	}

	public static Task ForAsync<T>(T fromInclusive, T toExclusive, ParallelOptions parallelOptions, Func<T, CancellationToken, ValueTask> body) where T : notnull, IBinaryInteger<T>
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, Action<TSource, ParallelLoopState, long> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState, long> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static Task ForEachAsync<TSource>(IEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask> body)
	{
		throw null;
	}

	public static Task ForEachAsync<TSource>(IEnumerable<TSource> source, CancellationToken cancellationToken, Func<TSource, CancellationToken, ValueTask> body)
	{
		throw null;
	}

	public static Task ForEachAsync<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TSource, CancellationToken, ValueTask> body)
	{
		throw null;
	}

	public static Task ForEachAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask> body)
	{
		throw null;
	}

	public static Task ForEachAsync<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken, Func<TSource, CancellationToken, ValueTask> body)
	{
		throw null;
	}

	public static Task ForEachAsync<TSource>(IAsyncEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TSource, CancellationToken, ValueTask> body)
	{
		throw null;
	}

	public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
	{
		throw null;
	}

	public static void Invoke(params Action[] actions)
	{
	}

	public static void Invoke(ParallelOptions parallelOptions, params Action[] actions)
	{
	}
}
