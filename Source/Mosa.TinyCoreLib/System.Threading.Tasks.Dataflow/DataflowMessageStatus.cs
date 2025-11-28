namespace System.Threading.Tasks.Dataflow;

public enum DataflowMessageStatus
{
	Accepted,
	Declined,
	Postponed,
	NotAvailable,
	DecliningPermanently
}
