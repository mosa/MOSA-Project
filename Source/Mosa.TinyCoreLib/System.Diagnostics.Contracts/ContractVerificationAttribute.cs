namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractVerificationAttribute(bool value) : Attribute
{
	public bool Value => value;
}
