using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks.Dataflow;

public sealed class BatchedJoinBlock<T1, T2> : IDataflowBlock, IReceivableSourceBlock<Tuple<IList<T1>, IList<T2>>>, ISourceBlock<Tuple<IList<T1>, IList<T2>>>
{
	public int BatchSize
	{
		get
		{
			throw null;
		}
	}

	public Task Completion
	{
		get
		{
			throw null;
		}
	}

	public int OutputCount
	{
		get
		{
			throw null;
		}
	}

	public ITargetBlock<T1> Target1
	{
		get
		{
			throw null;
		}
	}

	public ITargetBlock<T2> Target2
	{
		get
		{
			throw null;
		}
	}

	public BatchedJoinBlock(int batchSize)
	{
	}

	public BatchedJoinBlock(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public void Complete()
	{
	}

	public IDisposable LinkTo(ITargetBlock<Tuple<IList<T1>, IList<T2>>> target, DataflowLinkOptions linkOptions)
	{
		throw null;
	}

	void IDataflowBlock.Fault(Exception exception)
	{
	}

	Tuple<IList<T1>, IList<T2>> ISourceBlock<Tuple<IList<T1>, IList<T2>>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<IList<T1>, IList<T2>>> target, out bool messageConsumed)
	{
		throw null;
	}

	void ISourceBlock<Tuple<IList<T1>, IList<T2>>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<IList<T1>, IList<T2>>> target)
	{
	}

	bool ISourceBlock<Tuple<IList<T1>, IList<T2>>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<IList<T1>, IList<T2>>> target)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryReceive(Predicate<Tuple<IList<T1>, IList<T2>>>? filter, [NotNullWhen(true)] out Tuple<IList<T1>, IList<T2>>? item)
	{
		throw null;
	}

	public bool TryReceiveAll([NotNullWhen(true)] out IList<Tuple<IList<T1>, IList<T2>>>? items)
	{
		throw null;
	}
}
public sealed class BatchedJoinBlock<T1, T2, T3> : IDataflowBlock, IReceivableSourceBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>>, ISourceBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>>
{
	public int BatchSize
	{
		get
		{
			throw null;
		}
	}

	public Task Completion
	{
		get
		{
			throw null;
		}
	}

	public int OutputCount
	{
		get
		{
			throw null;
		}
	}

	public ITargetBlock<T1> Target1
	{
		get
		{
			throw null;
		}
	}

	public ITargetBlock<T2> Target2
	{
		get
		{
			throw null;
		}
	}

	public ITargetBlock<T3> Target3
	{
		get
		{
			throw null;
		}
	}

	public BatchedJoinBlock(int batchSize)
	{
	}

	public BatchedJoinBlock(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public void Complete()
	{
	}

	public IDisposable LinkTo(ITargetBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>> target, DataflowLinkOptions linkOptions)
	{
		throw null;
	}

	void IDataflowBlock.Fault(Exception exception)
	{
	}

	Tuple<IList<T1>, IList<T2>, IList<T3>> ISourceBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>> target, out bool messageConsumed)
	{
		throw null;
	}

	void ISourceBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>> target)
	{
	}

	bool ISourceBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<IList<T1>, IList<T2>, IList<T3>>> target)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryReceive(Predicate<Tuple<IList<T1>, IList<T2>, IList<T3>>>? filter, [NotNullWhen(true)] out Tuple<IList<T1>, IList<T2>, IList<T3>>? item)
	{
		throw null;
	}

	public bool TryReceiveAll([NotNullWhen(true)] out IList<Tuple<IList<T1>, IList<T2>, IList<T3>>>? items)
	{
		throw null;
	}
}
