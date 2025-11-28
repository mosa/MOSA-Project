using System.Collections.Generic;
using System.Globalization;

namespace System.Diagnostics.Eventing.Reader;

public class ProviderMetadata : IDisposable
{
	public string DisplayName
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<EventMetadata> Events
	{
		get
		{
			throw null;
		}
	}

	public Uri HelpLink
	{
		get
		{
			throw null;
		}
	}

	public Guid Id
	{
		get
		{
			throw null;
		}
	}

	public IList<EventKeyword> Keywords
	{
		get
		{
			throw null;
		}
	}

	public IList<EventLevel> Levels
	{
		get
		{
			throw null;
		}
	}

	public IList<EventLogLink> LogLinks
	{
		get
		{
			throw null;
		}
	}

	public string MessageFilePath
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

	public IList<EventOpcode> Opcodes
	{
		get
		{
			throw null;
		}
	}

	public string ParameterFilePath
	{
		get
		{
			throw null;
		}
	}

	public string ResourceFilePath
	{
		get
		{
			throw null;
		}
	}

	public IList<EventTask> Tasks
	{
		get
		{
			throw null;
		}
	}

	public ProviderMetadata(string providerName)
	{
	}

	public ProviderMetadata(string providerName, EventLogSession session, CultureInfo targetCultureInfo)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
