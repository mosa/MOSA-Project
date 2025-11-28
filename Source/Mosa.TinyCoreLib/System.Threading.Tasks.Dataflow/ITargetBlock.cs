namespace System.Threading.Tasks.Dataflow;

public interface ITargetBlock<in TInput> : IDataflowBlock
{
	DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput>? source, bool consumeToAccept);
}
