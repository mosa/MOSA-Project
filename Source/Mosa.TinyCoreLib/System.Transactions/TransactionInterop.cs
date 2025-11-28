namespace System.Transactions;

public static class TransactionInterop
{
	public static readonly Guid PromoterTypeDtc;

	public static IDtcTransaction GetDtcTransaction(Transaction transaction)
	{
		throw null;
	}

	public static byte[] GetExportCookie(Transaction transaction, byte[] whereabouts)
	{
		throw null;
	}

	public static Transaction GetTransactionFromDtcTransaction(IDtcTransaction transactionNative)
	{
		throw null;
	}

	public static Transaction GetTransactionFromExportCookie(byte[] cookie)
	{
		throw null;
	}

	public static Transaction GetTransactionFromTransmitterPropagationToken(byte[] propagationToken)
	{
		throw null;
	}

	public static byte[] GetTransmitterPropagationToken(Transaction transaction)
	{
		throw null;
	}

	public static byte[] GetWhereabouts()
	{
		throw null;
	}
}
