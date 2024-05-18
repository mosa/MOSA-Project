namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class DefaultParameterValueAttribute : Attribute
{
	public object? Value
	{
		get
		{
			throw null;
		}
	}

	public DefaultParameterValueAttribute(object? value)
	{
	}
}
