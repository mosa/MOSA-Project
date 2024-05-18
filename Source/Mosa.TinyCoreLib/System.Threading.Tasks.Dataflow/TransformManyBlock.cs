using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks.Dataflow;

public sealed class TransformManyBlock<TInput, TOutput> : IDataflowBlock, IPropagatorBlock<TInput, TOutput>, ISourceBlock<TOutput>, ITargetBlock<TInput>, IReceivableSourceBlock<TOutput>
{
	public Task Completion
	{
		get
		{
			throw null;
		}
	}

	public int InputCount
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

	public TransformManyBlock(Func<TInput, IAsyncEnumerable<TOutput>> transform)
	{
	}

	public TransformManyBlock(Func<TInput, IAsyncEnumerable<TOutput>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public TransformManyBlock(Func<TInput, IEnumerable<TOutput>> transform)
	{
	}

	public TransformManyBlock(Func<TInput, IEnumerable<TOutput>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public TransformManyBlock(Func<TInput, Task<IEnumerable<TOutput>>> transform)
	{
	}

	public TransformManyBlock(Func<TInput, Task<IEnumerable<TOutput>>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public void Complete()
	{
	}

	public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
	{
		throw null;
	}

	void IDataflowBlock.Fault(Exception exception)
	{
	}

	TOutput? ISourceBlock<TOutput>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
	{
		throw null;
	}

	void ISourceBlock<TOutput>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
	{
	}

	bool ISourceBlock<TOutput>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
	{
		throw null;
	}

	DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput>? source, bool consumeToAccept)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryReceive(Predicate<TOutput>? filter, [MaybeNullWhen(false)] out TOutput item)
	{
		throw null;
	}

	public bool TryReceiveAll([NotNullWhen(true)] out IList<TOutput>? items)
	{
		throw null;
	}
}
