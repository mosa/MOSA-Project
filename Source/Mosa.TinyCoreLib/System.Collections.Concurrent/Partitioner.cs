using System.Collections.Generic;

namespace System.Collections.Concurrent;

public static class Partitioner
{
	public static OrderablePartitioner<Tuple<int, int>> Create(int fromInclusive, int toExclusive)
	{
		throw null;
	}

	public static OrderablePartitioner<Tuple<int, int>> Create(int fromInclusive, int toExclusive, int rangeSize)
	{
		throw null;
	}

	public static OrderablePartitioner<Tuple<long, long>> Create(long fromInclusive, long toExclusive)
	{
		throw null;
	}

	public static OrderablePartitioner<Tuple<long, long>> Create(long fromInclusive, long toExclusive, long rangeSize)
	{
		throw null;
	}

	public static OrderablePartitioner<TSource> Create<TSource>(IEnumerable<TSource> source)
	{
		throw null;
	}

	public static OrderablePartitioner<TSource> Create<TSource>(IEnumerable<TSource> source, EnumerablePartitionerOptions partitionerOptions)
	{
		throw null;
	}

	public static OrderablePartitioner<TSource> Create<TSource>(IList<TSource> list, bool loadBalance)
	{
		throw null;
	}

	public static OrderablePartitioner<TSource> Create<TSource>(TSource[] array, bool loadBalance)
	{
		throw null;
	}
}
public abstract class Partitioner<TSource>
{
	public virtual bool SupportsDynamicPartitions
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<TSource> GetDynamicPartitions()
	{
		throw null;
	}

	public abstract IList<IEnumerator<TSource>> GetPartitions(int partitionCount);
}
