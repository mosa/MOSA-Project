using System.Collections.Generic;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogPropertySelector : IDisposable
{
	public EventLogPropertySelector(IEnumerable<string> propertyQueries)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
