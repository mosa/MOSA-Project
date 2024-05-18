namespace System.Threading.Tasks;

public enum TaskStatus
{
	Created,
	WaitingForActivation,
	WaitingToRun,
	Running,
	WaitingForChildrenToComplete,
	RanToCompletion,
	Canceled,
	Faulted
}
