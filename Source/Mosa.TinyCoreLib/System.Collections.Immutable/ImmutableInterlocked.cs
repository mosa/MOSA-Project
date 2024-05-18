using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable;

public static class ImmutableInterlocked
{
	public static TValue AddOrUpdate<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory) where TKey : notnull
	{
		throw null;
	}

	public static TValue AddOrUpdate<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory) where TKey : notnull
	{
		throw null;
	}

	public static void Enqueue<T>(ref ImmutableQueue<T> location, T value)
	{
	}

	public static TValue GetOrAdd<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, Func<TKey, TValue> valueFactory) where TKey : notnull
	{
		throw null;
	}

	public static TValue GetOrAdd<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue value) where TKey : notnull
	{
		throw null;
	}

	public static TValue GetOrAdd<TKey, TValue, TArg>(ref ImmutableDictionary<TKey, TValue> location, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument) where TKey : notnull
	{
		throw null;
	}

	public static ImmutableArray<T> InterlockedCompareExchange<T>(ref ImmutableArray<T> location, ImmutableArray<T> value, ImmutableArray<T> comparand)
	{
		throw null;
	}

	public static ImmutableArray<T> InterlockedExchange<T>(ref ImmutableArray<T> location, ImmutableArray<T> value)
	{
		throw null;
	}

	public static bool InterlockedInitialize<T>(ref ImmutableArray<T> location, ImmutableArray<T> value)
	{
		throw null;
	}

	public static void Push<T>(ref ImmutableStack<T> location, T value)
	{
	}

	public static bool TryAdd<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue value) where TKey : notnull
	{
		throw null;
	}

	public static bool TryDequeue<T>(ref ImmutableQueue<T> location, [MaybeNullWhen(false)] out T value)
	{
		throw null;
	}

	public static bool TryPop<T>(ref ImmutableStack<T> location, [MaybeNullWhen(false)] out T value)
	{
		throw null;
	}

	public static bool TryRemove<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, [MaybeNullWhen(false)] out TValue value) where TKey : notnull
	{
		throw null;
	}

	public static bool TryUpdate<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue newValue, TValue comparisonValue) where TKey : notnull
	{
		throw null;
	}

	public static bool Update<T>(ref T location, Func<T, T> transformer) where T : class?
	{
		throw null;
	}

	public static bool Update<T, TArg>(ref T location, Func<T, TArg, T> transformer, TArg transformerArgument) where T : class?
	{
		throw null;
	}

	public static bool Update<T>(ref ImmutableArray<T> location, Func<ImmutableArray<T>, ImmutableArray<T>> transformer)
	{
		throw null;
	}

	public static bool Update<T, TArg>(ref ImmutableArray<T> location, Func<ImmutableArray<T>, TArg, ImmutableArray<T>> transformer, TArg transformerArgument)
	{
		throw null;
	}
}
