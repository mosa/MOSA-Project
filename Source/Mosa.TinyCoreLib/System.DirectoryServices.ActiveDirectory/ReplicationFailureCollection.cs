using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReplicationFailureCollection : ReadOnlyCollectionBase
{
	public ReplicationFailure this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReplicationFailureCollection()
	{
	}

	public bool Contains(ReplicationFailure failure)
	{
		throw null;
	}

	public void CopyTo(ReplicationFailure[] failures, int index)
	{
	}

	public int IndexOf(ReplicationFailure failure)
	{
		throw null;
	}
}
