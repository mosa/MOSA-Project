namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class CompilerFeatureRequiredAttribute : Attribute
{
	public const string RefStructs = "RefStructs";

	public const string RequiredMembers = "RequiredMembers";

	public string FeatureName
	{
		get
		{
			throw null;
		}
	}

	public bool IsOptional
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public CompilerFeatureRequiredAttribute(string featureName)
	{
	}
}
