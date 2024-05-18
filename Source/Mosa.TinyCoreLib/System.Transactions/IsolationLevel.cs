namespace System.Transactions;

public enum IsolationLevel
{
	Serializable,
	RepeatableRead,
	ReadCommitted,
	ReadUncommitted,
	Snapshot,
	Chaos,
	Unspecified
}
