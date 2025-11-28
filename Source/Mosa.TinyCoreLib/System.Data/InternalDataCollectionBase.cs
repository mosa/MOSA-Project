using System.Collections;
using System.ComponentModel;

namespace System.Data;

public class InternalDataCollectionBase : ICollection, IEnumerable
{
	[Browsable(false)]
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	protected virtual ArrayList List
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public virtual void CopyTo(Array ar, int index)
	{
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}
}
