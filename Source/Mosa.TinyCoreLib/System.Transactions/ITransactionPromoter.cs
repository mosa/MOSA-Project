namespace System.Transactions;

public interface ITransactionPromoter
{
	byte[]? Promote();
}
