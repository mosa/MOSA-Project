namespace System.Diagnostics.Tracing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public class EventDataAttribute : Attribute
{
	public string? Name
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
