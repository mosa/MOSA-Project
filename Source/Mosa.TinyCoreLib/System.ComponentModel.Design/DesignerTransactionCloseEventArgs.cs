namespace System.ComponentModel.Design;

public class DesignerTransactionCloseEventArgs : EventArgs
{
	public bool LastTransaction
	{
		get
		{
			throw null;
		}
	}

	public bool TransactionCommitted
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This constructor has been deprecated. Use DesignerTransactionCloseEventArgs(bool, bool) instead.")]
	public DesignerTransactionCloseEventArgs(bool commit)
	{
	}

	public DesignerTransactionCloseEventArgs(bool commit, bool lastTransaction)
	{
	}
}
