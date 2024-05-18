using System.Collections.Generic;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogConfiguration : IDisposable
{
	public bool IsClassicLog
	{
		get
		{
			throw null;
		}
	}

	public bool IsEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string LogFilePath
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventLogIsolation LogIsolation
	{
		get
		{
			throw null;
		}
	}

	public EventLogMode LogMode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string LogName
	{
		get
		{
			throw null;
		}
	}

	public EventLogType LogType
	{
		get
		{
			throw null;
		}
	}

	public long MaximumSizeInBytes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string OwningProviderName
	{
		get
		{
			throw null;
		}
	}

	public int? ProviderBufferSize
	{
		get
		{
			throw null;
		}
	}

	public Guid? ProviderControlGuid
	{
		get
		{
			throw null;
		}
	}

	public long? ProviderKeywords
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int? ProviderLatency
	{
		get
		{
			throw null;
		}
	}

	public int? ProviderLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int? ProviderMaximumNumberOfBuffers
	{
		get
		{
			throw null;
		}
	}

	public int? ProviderMinimumNumberOfBuffers
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<string> ProviderNames
	{
		get
		{
			throw null;
		}
	}

	public string SecurityDescriptor
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventLogConfiguration(string logName)
	{
	}

	public EventLogConfiguration(string logName, EventLogSession session)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void SaveChanges()
	{
	}
}
