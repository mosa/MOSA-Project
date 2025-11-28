using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Transactions;

public static class TransactionManager
{
	public static TimeSpan DefaultTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static HostCurrentTransactionCallback? HostCurrentCallback
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public static TimeSpan MaximumTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool ImplicitDistributedTransactions
	{
		get; [SupportedOSPlatform("windows")]
		[RequiresUnreferencedCode("Distributed transactions support may not be compatible with trimming. If your program creates a distributed transaction via System.Transactions, the correctness of the application cannot be guaranteed after trimming.")]
		set;
	}

	public static event TransactionStartedEventHandler? DistributedTransactionStarted
	{
		add
		{
		}
		remove
		{
		}
	}

	public static void RecoveryComplete(Guid resourceManagerIdentifier)
	{
	}

	public static Enlistment Reenlist(Guid resourceManagerIdentifier, byte[] recoveryInformation, IEnlistmentNotification enlistmentNotification)
	{
		throw null;
	}
}
