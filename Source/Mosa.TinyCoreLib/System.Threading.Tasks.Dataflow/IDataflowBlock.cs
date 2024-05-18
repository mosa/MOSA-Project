namespace System.Threading.Tasks.Dataflow;

public interface IDataflowBlock
{
	Task Completion { get; }

	void Complete();

	void Fault(Exception exception);
}
