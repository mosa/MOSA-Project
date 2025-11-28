using System.ComponentModel;

namespace System.Resources;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class NeutralResourcesLanguageAttribute : Attribute
{
	public string CultureName
	{
		get
		{
			throw null;
		}
	}

	public UltimateResourceFallbackLocation Location
	{
		get
		{
			throw null;
		}
	}

	public NeutralResourcesLanguageAttribute(string cultureName)
	{
	}

	public NeutralResourcesLanguageAttribute(string cultureName, UltimateResourceFallbackLocation location)
	{
	}
}
