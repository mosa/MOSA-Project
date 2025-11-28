using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks.Dataflow;

public sealed class BatchBlock<T> : IDataflowBlock, IPropagatorBlock<T, T[]>, ISourceBlock<T[]>, ITargetBlock<T>, IReceivableSourceBlock<T[]>
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

	public BatchBlock(int batchSize)
	{
	}

	public BatchBlock(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public void Complete()
	{
	}

	public IDisposable LinkTo(ITargetBlock<T[]> target, DataflowLinkOptions linkOptions)
	{
		throw null;
	}

	void IDataflowBlock.Fault(Exception exception)
	{
	}

	T[]? ISourceBlock<T[]>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target, out bool messageConsumed)
	{
		throw null;
	}

	void ISourceBlock<T[]>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target)
	{
	}

	bool ISourceBlock<T[]>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target)
	{
		throw null;
	}

	DataflowMessageStatus ITargetBlock<T>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T>? source, bool consumeToAccept)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public void TriggerBatch()
	{
	}

	public bool TryReceive(Predicate<T[]>? filter, [NotNullWhen(true)] out T[]? item)
	{
		throw null;
	}

	public bool TryReceiveAll([NotNullWhen(true)] out IList<T[]>? items)
	{
		throw null;
	}
}
