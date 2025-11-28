namespace System.Transactions;

public interface ISinglePhaseNotification : IEnlistmentNotification
{
	void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment);
}
