namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InitializationEventAttribute : Attribute
{
	public string EventName
	{
		get
		{
			throw null;
		}
	}

	public InitializationEventAttribute(string eventName)
	{
	}
}
