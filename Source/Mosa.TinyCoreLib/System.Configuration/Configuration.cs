using System.Runtime.Versioning;

namespace System.Configuration;

public sealed class Configuration
{
	public AppSettingsSection AppSettings
	{
		get
		{
			throw null;
		}
	}

	public Func<string, string> AssemblyStringTransformer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConnectionStringsSection ConnectionStrings
	{
		get
		{
			throw null;
		}
	}

	public ContextInformation EvaluationContext
	{
		get
		{
			throw null;
		}
	}

	public string FilePath
	{
		get
		{
			throw null;
		}
	}

	public bool HasFile
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationLocationCollection Locations
	{
		get
		{
			throw null;
		}
	}

	public bool NamespaceDeclared
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConfigurationSectionGroup RootSectionGroup
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

	public FrameworkName TargetFramework
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Func<string, string> TypeStringTransformer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal Configuration()
	{
	}

	public ConfigurationSection GetSection(string sectionName)
	{
		throw null;
	}

	public ConfigurationSectionGroup GetSectionGroup(string sectionGroupName)
	{
		throw null;
	}

	public void Save()
	{
	}

	public void Save(ConfigurationSaveMode saveMode)
	{
	}

	public void Save(ConfigurationSaveMode saveMode, bool forceSaveAll)
	{
	}

	public void SaveAs(string filename)
	{
	}

	public void SaveAs(string filename, ConfigurationSaveMode saveMode)
	{
	}

	public void SaveAs(string filename, ConfigurationSaveMode saveMode, bool forceSaveAll)
	{
	}
}
