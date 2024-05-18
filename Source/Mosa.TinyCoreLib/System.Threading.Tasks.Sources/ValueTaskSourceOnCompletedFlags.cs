namespace System.Threading.Tasks.Sources;

[Flags]
public enum ValueTaskSourceOnCompletedFlags
{
	None = 0,
	UseSchedulingContext = 1,
	FlowExecutionContext = 2
}
