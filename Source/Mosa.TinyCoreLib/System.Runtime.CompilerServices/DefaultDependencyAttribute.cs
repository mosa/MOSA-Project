namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class DefaultDependencyAttribute : Attribute
{
	public LoadHint LoadHint
	{
		get
		{
			throw null;
		}
	}

	public DefaultDependencyAttribute(LoadHint loadHintArgument)
	{
	}
}
