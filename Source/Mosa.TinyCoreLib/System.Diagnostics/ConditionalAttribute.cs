namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class ConditionalAttribute : Attribute
{
	public string ConditionString
	{
		get
		{
			throw null;
		}
	}

	public ConditionalAttribute(string conditionString)
	{
	}
}
