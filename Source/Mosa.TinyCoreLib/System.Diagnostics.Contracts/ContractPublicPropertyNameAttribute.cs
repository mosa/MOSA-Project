namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Field)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractPublicPropertyNameAttribute(string name) : Attribute
{
	public string Name => name;
}
