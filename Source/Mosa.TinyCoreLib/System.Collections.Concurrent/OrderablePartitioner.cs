using System.Collections.Generic;

namespace System.Collections.Concurrent;

public abstract class OrderablePartitioner<TSource> : Partitioner<TSource>
{
	public bool KeysNormalized
	{
		get
		{
			throw null;
		}
	}

	public bool KeysOrderedAcrossPartitions
	{
		get
		{
			throw null;
		}
	}

	public bool KeysOrderedInEachPartition
	{
		get
		{
			throw null;
		}
	}

	protected OrderablePartitioner(bool keysOrderedInEachPartition, bool keysOrderedAcrossPartitions, bool keysNormalized)
	{
	}

	public override IEnumerable<TSource> GetDynamicPartitions()
	{
		throw null;
	}

	public virtual IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions()
	{
		throw null;
	}

	public abstract IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount);

	public override IList<IEnumerator<TSource>> GetPartitions(int partitionCount)
	{
		throw null;
	}
}
