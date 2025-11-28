namespace System.DirectoryServices.ActiveDirectory;

public abstract class ActiveDirectoryPartition : IDisposable
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract DirectoryEntry GetDirectoryEntry();

	public override string ToString()
	{
		throw null;
	}
}
