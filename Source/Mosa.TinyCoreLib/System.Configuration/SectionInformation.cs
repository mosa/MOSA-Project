namespace System.Configuration;

public sealed class SectionInformation
{
	public ConfigurationAllowDefinition AllowDefinition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConfigurationAllowExeDefinition AllowExeDefinition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AllowLocation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AllowOverride
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ConfigSource
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ForceSave
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool InheritInChildApplications
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public bool IsLocked
	{
		get
		{
			throw null;
		}
	}

	public bool IsProtected
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

	public OverrideMode OverrideMode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OverrideMode OverrideModeDefault
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OverrideMode OverrideModeEffective
	{
		get
		{
			throw null;
		}
	}

	public ProtectedConfigurationProvider ProtectionProvider
	{
		get
		{
			throw null;
		}
	}

	public bool RequirePermission
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RestartOnExternalChanges
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string SectionName
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

	internal SectionInformation()
	{
	}

	public void ForceDeclaration()
	{
	}

	public void ForceDeclaration(bool force)
	{
	}

	public ConfigurationSection GetParentSection()
	{
		throw null;
	}

	public string GetRawXml()
	{
		throw null;
	}

	public void ProtectSection(string protectionProvider)
	{
	}

	public void RevertToParent()
	{
	}

	public void SetRawXml(string rawXml)
	{
	}

	public void UnprotectSection()
	{
	}
}
