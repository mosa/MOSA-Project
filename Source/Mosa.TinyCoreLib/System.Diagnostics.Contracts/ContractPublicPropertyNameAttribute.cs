namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Field)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractPublicPropertyNameAttribute : Attribute
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public ContractPublicPropertyNameAttribute(string name)
	{
	}
}
