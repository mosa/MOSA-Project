using System.ComponentModel;

namespace System.Management;

[TypeConverter(typeof(ManagementQueryConverter))]
public abstract class ManagementQuery : ICloneable
{
	public virtual string QueryLanguage
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string QueryString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ManagementQuery()
	{
	}

	public abstract object Clone();

	protected internal virtual void ParseQuery(string query)
	{
	}
}
