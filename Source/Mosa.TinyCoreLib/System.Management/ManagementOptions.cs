using System.ComponentModel;

namespace System.Management;

[TypeConverter(typeof(ExpandableObjectConverter))]
public abstract class ManagementOptions : ICloneable
{
	public static readonly TimeSpan InfiniteTimeout;

	public ManagementNamedValueCollection Context
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ManagementOptions()
	{
	}

	public abstract object Clone();
}
