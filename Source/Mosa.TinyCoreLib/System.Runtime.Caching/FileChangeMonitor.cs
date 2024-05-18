using System.Collections.ObjectModel;

namespace System.Runtime.Caching;

public abstract class FileChangeMonitor : ChangeMonitor
{
	public abstract ReadOnlyCollection<string> FilePaths { get; }

	public abstract DateTimeOffset LastModified { get; }
}
