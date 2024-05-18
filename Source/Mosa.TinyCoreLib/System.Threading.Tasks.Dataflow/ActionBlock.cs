namespace System.Threading.Tasks.Dataflow;

public sealed class ActionBlock<TInput> : IDataflowBlock, ITargetBlock<TInput>
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

	public ActionBlock(Action<TInput> action)
	{
	}

	public ActionBlock(Action<TInput> action, ExecutionDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public ActionBlock(Func<TInput, Task> action)
	{
	}

	public ActionBlock(Func<TInput, Task> action, ExecutionDataflowBlockOptions dataflowBlockOptions)
	{
	}

	public void Complete()
	{
	}

	public bool Post(TInput item)
	{
		throw null;
	}

	void IDataflowBlock.Fault(Exception exception)
	{
	}

	DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput>? source, bool consumeToAccept)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
