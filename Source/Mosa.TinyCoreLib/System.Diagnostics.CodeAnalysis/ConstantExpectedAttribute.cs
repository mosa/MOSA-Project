namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class ConstantExpectedAttribute : Attribute
{
	public object? Max
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object? Min
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
