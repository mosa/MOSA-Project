namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SwitchLevelAttribute : Attribute
{
	public Type SwitchLevelType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SwitchLevelAttribute(Type switchLevelType)
	{
	}
}
