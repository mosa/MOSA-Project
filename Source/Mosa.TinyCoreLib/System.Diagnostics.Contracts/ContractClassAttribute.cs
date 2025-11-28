namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
[Conditional("DEBUG")]
public sealed class ContractClassAttribute(Type typeContainingContracts) : Attribute
{
	public Type TypeContainingContracts => typeContainingContracts;
}
