namespace System.Diagnostics.Tracing;

[AttributeUsage(AttributeTargets.Property)]
public class EventFieldAttribute : Attribute
{
	public EventFieldFormat Format
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventFieldTags Tags
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
