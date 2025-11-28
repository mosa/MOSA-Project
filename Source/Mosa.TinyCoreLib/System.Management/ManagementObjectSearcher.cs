using System.ComponentModel;

namespace System.Management;

[ToolboxItem(false)]
public class ManagementObjectSearcher : Component
{
	public EnumerationOptions Options
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ObjectQuery Query
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementScope Scope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementObjectSearcher()
	{
	}

	public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query)
	{
	}

	public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query, EnumerationOptions options)
	{
	}

	public ManagementObjectSearcher(ObjectQuery query)
	{
	}

	public ManagementObjectSearcher(string queryString)
	{
	}

	public ManagementObjectSearcher(string scope, string queryString)
	{
	}

	public ManagementObjectSearcher(string scope, string queryString, EnumerationOptions options)
	{
	}

	public ManagementObjectCollection Get()
	{
		throw null;
	}

	public void Get(ManagementOperationObserver watcher)
	{
	}
}
