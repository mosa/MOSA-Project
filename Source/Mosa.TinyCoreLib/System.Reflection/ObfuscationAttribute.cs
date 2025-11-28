namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
public sealed class ObfuscationAttribute : Attribute
{
	public bool ApplyToMembers
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Exclude
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Feature
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool StripAfterObfuscation
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
