namespace System.Threading.Tasks.Dataflow;

public interface IPropagatorBlock<in TInput, out TOutput> : IDataflowBlock, ISourceBlock<TOutput>, ITargetBlock<TInput>
{
}
