using System.Collections;

namespace System.Diagnostics;

public class EventLogEntryCollection : ICollection, IEnumerable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual EventLogEntry this[int index]
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	internal EventLogEntryCollection()
	{
	}

	public void CopyTo(EventLogEntry[] entries, int index)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}
}
