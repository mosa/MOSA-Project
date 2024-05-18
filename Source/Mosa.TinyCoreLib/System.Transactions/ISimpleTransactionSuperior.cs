namespace System.Transactions;

public interface ISimpleTransactionSuperior : ITransactionPromoter
{
	void Rollback();
}
