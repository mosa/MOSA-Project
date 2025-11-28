namespace System.Transactions;

public sealed class SubordinateTransaction : Transaction
{
	public SubordinateTransaction(IsolationLevel isoLevel, ISimpleTransactionSuperior superior)
	{
	}
}
