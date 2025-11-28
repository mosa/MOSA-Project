using System.Runtime.InteropServices;

namespace System.Transactions;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IDtcTransaction
{
	void Abort(IntPtr reason, int retaining, int async);

	void Commit(int retaining, int commitType, int reserved);

	void GetTransactionInfo(IntPtr transactionInformation);
}
