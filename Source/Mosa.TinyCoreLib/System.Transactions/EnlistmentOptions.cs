namespace System.Transactions;

[Flags]
public enum EnlistmentOptions
{
	None = 0,
	EnlistDuringPrepareRequired = 1
}
