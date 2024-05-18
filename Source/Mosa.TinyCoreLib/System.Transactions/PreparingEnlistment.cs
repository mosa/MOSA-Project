namespace System.Transactions;

public class PreparingEnlistment : Enlistment
{
	internal PreparingEnlistment()
	{
	}

	public void ForceRollback()
	{
	}

	public void ForceRollback(Exception? e)
	{
	}

	public void Prepared()
	{
	}

	public byte[] RecoveryInformation()
	{
		throw null;
	}
}
