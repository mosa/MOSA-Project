namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
public sealed class DateTimeConstantAttribute : CustomConstantAttribute
{
	public override object Value
	{
		get
		{
			throw null;
		}
	}

	public DateTimeConstantAttribute(long ticks)
	{
	}
}
