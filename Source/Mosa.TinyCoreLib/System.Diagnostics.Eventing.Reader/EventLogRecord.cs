using System.Collections.Generic;
using System.Security.Principal;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogRecord : EventRecord
{
	public override Guid? ActivityId
	{
		get
		{
			throw null;
		}
	}

	public override EventBookmark Bookmark
	{
		get
		{
			throw null;
		}
	}

	public string ContainerLog
	{
		get
		{
			throw null;
		}
	}

	public override int Id
	{
		get
		{
			throw null;
		}
	}

	public override long? Keywords
	{
		get
		{
			throw null;
		}
	}

	public override IEnumerable<string> KeywordsDisplayNames
	{
		get
		{
			throw null;
		}
	}

	public override byte? Level
	{
		get
		{
			throw null;
		}
	}

	public override string LevelDisplayName
	{
		get
		{
			throw null;
		}
	}

	public override string LogName
	{
		get
		{
			throw null;
		}
	}

	public override string MachineName
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<int> MatchedQueryIds
	{
		get
		{
			throw null;
		}
	}

	public override short? Opcode
	{
		get
		{
			throw null;
		}
	}

	public override string OpcodeDisplayName
	{
		get
		{
			throw null;
		}
	}

	public override int? ProcessId
	{
		get
		{
			throw null;
		}
	}

	public override IList<EventProperty> Properties
	{
		get
		{
			throw null;
		}
	}

	public override Guid? ProviderId
	{
		get
		{
			throw null;
		}
	}

	public override string ProviderName
	{
		get
		{
			throw null;
		}
	}

	public override int? Qualifiers
	{
		get
		{
			throw null;
		}
	}

	public override long? RecordId
	{
		get
		{
			throw null;
		}
	}

	public override Guid? RelatedActivityId
	{
		get
		{
			throw null;
		}
	}

	public override int? Task
	{
		get
		{
			throw null;
		}
	}

	public override string TaskDisplayName
	{
		get
		{
			throw null;
		}
	}

	public override int? ThreadId
	{
		get
		{
			throw null;
		}
	}

	public override DateTime? TimeCreated
	{
		get
		{
			throw null;
		}
	}

	public override SecurityIdentifier UserId
	{
		get
		{
			throw null;
		}
	}

	public override byte? Version
	{
		get
		{
			throw null;
		}
	}

	internal EventLogRecord()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override string FormatDescription()
	{
		throw null;
	}

	public override string FormatDescription(IEnumerable<object> values)
	{
		throw null;
	}

	public IList<object> GetPropertyValues(EventLogPropertySelector propertySelector)
	{
		throw null;
	}

	public override string ToXml()
	{
		throw null;
	}
}
