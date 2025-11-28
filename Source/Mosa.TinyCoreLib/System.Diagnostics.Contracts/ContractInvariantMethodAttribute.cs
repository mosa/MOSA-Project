namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractInvariantMethodAttribute : Attribute;