namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class ConditionalAttribute(string conditionString) : Attribute
{
	public string ConditionString { get; } = conditionString;
}
