namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
public sealed class NotNullIfNotNullAttribute : Attribute
{
	public string ParameterName
	{
		get
		{
			throw null;
		}
	}

	public NotNullIfNotNullAttribute(string parameterName)
	{
	}
}
