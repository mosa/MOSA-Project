using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReplicationCursorCollection : ReadOnlyCollectionBase
{
	public ReplicationCursor this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReplicationCursorCollection()
	{
	}

	public bool Contains(ReplicationCursor cursor)
	{
		throw null;
	}

	public void CopyTo(ReplicationCursor[] values, int index)
	{
	}

	public int IndexOf(ReplicationCursor cursor)
	{
		throw null;
	}
}
