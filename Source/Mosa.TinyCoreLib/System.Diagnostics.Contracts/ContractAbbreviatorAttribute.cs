namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractAbbreviatorAttribute : Attribute;