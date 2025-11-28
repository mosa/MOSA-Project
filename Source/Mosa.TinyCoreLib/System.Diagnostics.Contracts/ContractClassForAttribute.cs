namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractClassForAttribute(Type typeContractsAreFor) : Attribute
{
	public Type TypeContractsAreFor => typeContractsAreFor;
}
