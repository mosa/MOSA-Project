namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class NotNullWhenAttribute : Attribute
{
	public bool ReturnValue
	{
		get
		{
			throw null;
		}
	}

	public NotNullWhenAttribute(bool returnValue)
	{
	}
}
