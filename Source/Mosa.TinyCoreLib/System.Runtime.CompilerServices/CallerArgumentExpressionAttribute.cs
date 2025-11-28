namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class CallerArgumentExpressionAttribute : Attribute
{
	public string ParameterName
	{
		get
		{
			throw null;
		}
	}

	public CallerArgumentExpressionAttribute(string parameterName)
	{
	}
}
