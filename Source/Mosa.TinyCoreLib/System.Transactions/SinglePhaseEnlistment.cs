namespace System.Transactions;

public class SinglePhaseEnlistment : Enlistment
{
	internal SinglePhaseEnlistment()
	{
	}

	public void Aborted()
	{
	}

	public void Aborted(Exception? e)
	{
	}

	public void Committed()
	{
	}

	public void InDoubt()
	{
	}

	public void InDoubt(Exception? e)
	{
	}
}
