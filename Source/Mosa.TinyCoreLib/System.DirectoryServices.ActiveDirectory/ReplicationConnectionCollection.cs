using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReplicationConnectionCollection : ReadOnlyCollectionBase
{
	public ReplicationConnection this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReplicationConnectionCollection()
	{
	}

	public bool Contains(ReplicationConnection connection)
	{
		throw null;
	}

	public void CopyTo(ReplicationConnection[] connections, int index)
	{
	}

	public int IndexOf(ReplicationConnection connection)
	{
		throw null;
	}
}
