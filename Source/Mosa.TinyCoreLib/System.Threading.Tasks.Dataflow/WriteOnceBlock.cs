using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks.Dataflow;

public sealed class WriteOnceBlock<T> : IDataflowBlock, IPropagatorBlock<T, T>, ISourceBlock<T>, ITargetBlock<T>, IReceivableSourceBlock<T>
{
	public Task Completion
	{
		get
		{
			throw null;
		}
	}

	public WriteOnceBlock(Func<T, T>? cloningFunction)
	{
	}

	public WriteOnceBlock(Func<T, T>? cloningFunction, DataflowBlockOptions dataflowBlockOptions)
	{
	}

	public void Complete()
	{
	}

	public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
	{
		throw null;
	}

	void IDataflowBlock.Fault(Exception exception)
	{
	}

	bool IReceivableSourceBlock<T>.TryReceiveAll([NotNullWhen(true)] out IList<T>? items)
	{
		throw null;
	}

	T? ISourceBlock<T>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
	{
		throw null;
	}

	void ISourceBlock<T>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
	{
	}

	bool ISourceBlock<T>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
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

	public bool TryReceive(Predicate<T>? filter, [MaybeNullWhen(false)] out T item)
	{
		throw null;
	}
}
