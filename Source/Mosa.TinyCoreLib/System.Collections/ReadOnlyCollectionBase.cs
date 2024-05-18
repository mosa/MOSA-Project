namespace System.Collections;

public abstract class ReadOnlyCollectionBase : ICollection, IEnumerable
{
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	protected ArrayList InnerList
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

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}
}
