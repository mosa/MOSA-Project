using System.Runtime.Versioning;

namespace System.Transactions;

[UnsupportedOSPlatform("browser")]
public sealed class TransactionScope : IDisposable
{
	public TransactionScope()
	{
	}

	public TransactionScope(Transaction transactionToUse)
	{
	}

	public TransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout)
	{
	}

	public TransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout, EnterpriseServicesInteropOption interopOption)
	{
	}

	public TransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout, TransactionScopeAsyncFlowOption asyncFlowOption)
	{
	}

	public TransactionScope(Transaction transactionToUse, TransactionScopeAsyncFlowOption asyncFlowOption)
	{
	}

	public TransactionScope(TransactionScopeAsyncFlowOption asyncFlowOption)
	{
	}

	public TransactionScope(TransactionScopeOption scopeOption)
	{
	}

	public TransactionScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout)
	{
	}

	public TransactionScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout, TransactionScopeAsyncFlowOption asyncFlowOption)
	{
	}

	public TransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
	{
	}

	public TransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, EnterpriseServicesInteropOption interopOption)
	{
	}

	public TransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, TransactionScopeAsyncFlowOption asyncFlowOption)
	{
	}

	public TransactionScope(TransactionScopeOption scopeOption, TransactionScopeAsyncFlowOption asyncFlowOption)
	{
	}

	public void Complete()
	{
	}

	public void Dispose()
	{
	}
}
