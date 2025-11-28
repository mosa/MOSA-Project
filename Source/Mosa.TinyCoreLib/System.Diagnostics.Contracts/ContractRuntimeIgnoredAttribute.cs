namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractRuntimeIgnoredAttribute : Attribute;