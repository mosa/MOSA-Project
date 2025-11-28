namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
public sealed class RequiresPreviewFeaturesAttribute : Attribute
{
	public string? Message
	{
		get
		{
			throw null;
		}
	}

	public string? Url
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RequiresPreviewFeaturesAttribute()
	{
	}

	public RequiresPreviewFeaturesAttribute(string? message)
	{
	}
}
