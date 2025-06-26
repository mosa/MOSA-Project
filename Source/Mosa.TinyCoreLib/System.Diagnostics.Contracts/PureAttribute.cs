namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
[Conditional("CONTRACTS_FULL")]
public sealed class PureAttribute : Attribute;
