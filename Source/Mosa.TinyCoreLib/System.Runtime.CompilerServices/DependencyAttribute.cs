namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class DependencyAttribute : Attribute
{
	public string DependentAssembly
	{
		get
		{
			throw null;
		}
	}

	public LoadHint LoadHint
	{
		get
		{
			throw null;
		}
	}

	public DependencyAttribute(string dependentAssemblyArgument, LoadHint loadHintArgument)
	{
	}
}
