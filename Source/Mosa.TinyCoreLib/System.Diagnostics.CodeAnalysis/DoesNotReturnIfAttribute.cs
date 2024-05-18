namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class DoesNotReturnIfAttribute : Attribute
{
	public bool ParameterValue
	{
		get
		{
			throw null;
		}
	}

	public DoesNotReturnIfAttribute(bool parameterValue)
	{
	}
}
