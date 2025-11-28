using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Runtime.Caching;

public sealed class HostFileChangeMonitor : FileChangeMonitor
{
	public override ReadOnlyCollection<string> FilePaths
	{
		get
		{
			throw null;
		}
	}

	public override DateTimeOffset LastModified
	{
		get
		{
			throw null;
		}
	}

	public override string UniqueId
	{
		get
		{
			throw null;
		}
	}

	public HostFileChangeMonitor(IList<string> filePaths)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}
}
