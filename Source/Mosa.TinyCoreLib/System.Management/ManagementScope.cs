using System.ComponentModel;

namespace System.Management;

[TypeConverter(typeof(ManagementScopeConverter))]
public class ManagementScope : ICloneable
{
	public bool IsConnected
	{
		get
		{
			throw null;
		}
	}

	public ConnectionOptions Options
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementPath Path
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementScope()
	{
	}

	public ManagementScope(ManagementPath path)
	{
	}

	public ManagementScope(ManagementPath path, ConnectionOptions options)
	{
	}

	public ManagementScope(string path)
	{
	}

	public ManagementScope(string path, ConnectionOptions options)
	{
	}

	public ManagementScope Clone()
	{
		throw null;
	}

	public void Connect()
	{
	}

	object ICloneable.Clone()
	{
		throw null;
	}
}
