namespace System.IO;

public class RenamedEventArgs : FileSystemEventArgs
{
	public string OldFullPath
	{
		get
		{
			throw null;
		}
	}

	public string? OldName
	{
		get
		{
			throw null;
		}
	}

	public RenamedEventArgs(WatcherChangeTypes changeType, string directory, string? name, string? oldName)
		: base((WatcherChangeTypes)0, null, null)
	{
	}
}
