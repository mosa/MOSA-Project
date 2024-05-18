using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.IO;

public class FileSystemWatcher : Component, ISupportInitialize
{
	public bool EnableRaisingEvents
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Filter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Collection<string> Filters
	{
		get
		{
			throw null;
		}
	}

	public bool IncludeSubdirectories
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int InternalBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public NotifyFilters NotifyFilter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Editor("System.Diagnostics.Design.FSWPathEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public string Path
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override ISite? Site
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ISynchronizeInvoke? SynchronizingObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event FileSystemEventHandler? Changed
	{
		add
		{
		}
		remove
		{
		}
	}

	public event FileSystemEventHandler? Created
	{
		add
		{
		}
		remove
		{
		}
	}

	public event FileSystemEventHandler? Deleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ErrorEventHandler? Error
	{
		add
		{
		}
		remove
		{
		}
	}

	public event RenamedEventHandler? Renamed
	{
		add
		{
		}
		remove
		{
		}
	}

	public FileSystemWatcher()
	{
	}

	public FileSystemWatcher(string path)
	{
	}

	public FileSystemWatcher(string path, string filter)
	{
	}

	public void BeginInit()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public void EndInit()
	{
	}

	protected void OnChanged(FileSystemEventArgs e)
	{
	}

	protected void OnCreated(FileSystemEventArgs e)
	{
	}

	protected void OnDeleted(FileSystemEventArgs e)
	{
	}

	protected void OnError(ErrorEventArgs e)
	{
	}

	protected void OnRenamed(RenamedEventArgs e)
	{
	}

	public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
	{
		throw null;
	}

	public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
	{
		throw null;
	}

	public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, TimeSpan timeout)
	{
		throw null;
	}
}
