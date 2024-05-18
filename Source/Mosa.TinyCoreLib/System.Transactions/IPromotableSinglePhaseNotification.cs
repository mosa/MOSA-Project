namespace System.Transactions;

public interface IPromotableSinglePhaseNotification : ITransactionPromoter
{
	void Initialize();

	void Rollback(SinglePhaseEnlistment singlePhaseEnlistment);

	void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment);
}
