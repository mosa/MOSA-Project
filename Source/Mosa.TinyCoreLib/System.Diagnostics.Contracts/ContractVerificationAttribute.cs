namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractVerificationAttribute : Attribute
{
	public bool Value
	{
		get
		{
			throw null;
		}
	}

	public ContractVerificationAttribute(bool value)
	{
	}
}
