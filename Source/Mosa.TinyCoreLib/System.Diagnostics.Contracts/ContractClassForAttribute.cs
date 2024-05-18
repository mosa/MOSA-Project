namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractClassForAttribute : Attribute
{
	public Type TypeContractsAreFor
	{
		get
		{
			throw null;
		}
	}

	public ContractClassForAttribute(Type typeContractsAreFor)
	{
	}
}
