using System.ComponentModel;

namespace System.Management;

[TypeConverter(typeof(ManagementPathConverter))]
public class ManagementPath : ICloneable
{
	[RefreshProperties(RefreshProperties.All)]
	public string ClassName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static ManagementPath DefaultPath
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsClass
	{
		get
		{
			throw null;
		}
	}

	public bool IsInstance
	{
		get
		{
			throw null;
		}
	}

	public bool IsSingleton
	{
		get
		{
			throw null;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	public string NamespacePath
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	public string Path
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	public string RelativePath
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	public string Server
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementPath()
	{
	}

	public ManagementPath(string path)
	{
	}

	public ManagementPath Clone()
	{
		throw null;
	}

	public void SetAsClass()
	{
	}

	public void SetAsSingleton()
	{
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
