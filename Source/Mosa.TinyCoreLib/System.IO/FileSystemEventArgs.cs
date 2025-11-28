namespace System.IO;

public class FileSystemEventArgs : EventArgs
{
	public WatcherChangeTypes ChangeType
	{
		get
		{
			throw null;
		}
	}

	public string FullPath
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string? name)
	{
	}
}
