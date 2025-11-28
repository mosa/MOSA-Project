namespace System.Threading.Tasks.Dataflow;

public interface ISourceBlock<out TOutput> : IDataflowBlock
{
	TOutput? ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed);

	IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions);

	void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);

	bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);
}
