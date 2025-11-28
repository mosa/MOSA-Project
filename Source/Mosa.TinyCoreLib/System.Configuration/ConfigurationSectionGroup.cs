using System.Runtime.Versioning;

namespace System.Configuration;

public class ConfigurationSectionGroup
{
	public bool IsDeclarationRequired
	{
		get
		{
			throw null;
		}
	}

	public bool IsDeclared
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string SectionGroupName
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationSectionGroupCollection SectionGroups
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationSectionCollection Sections
	{
		get
		{
			throw null;
		}
	}

	public string Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public void ForceDeclaration()
	{
	}

	public void ForceDeclaration(bool force)
	{
	}

	protected virtual bool ShouldSerializeSectionGroupInTargetVersion(FrameworkName targetFramework)
	{
		throw null;
	}
}
