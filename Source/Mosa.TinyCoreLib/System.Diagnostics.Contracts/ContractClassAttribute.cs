namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
[Conditional("DEBUG")]
public sealed class ContractClassAttribute : Attribute
{
	public Type TypeContainingContracts
	{
		get
		{
			throw null;
		}
	}

	public ContractClassAttribute(Type typeContainingContracts)
	{
	}
}
