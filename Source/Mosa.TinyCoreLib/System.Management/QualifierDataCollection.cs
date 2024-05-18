using System.Collections;

namespace System.Management;

public class QualifierDataCollection : ICollection, IEnumerable
{
	public class QualifierDataEnumerator : IEnumerator
	{
		public QualifierData Current
		{
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		internal QualifierDataEnumerator()
		{
		}

		public bool MoveNext()
		{
			throw null;
		}

		public void Reset()
		{
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public virtual QualifierData this[string qualifierName]
	{
		get
		{
			throw null;
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	internal QualifierDataCollection()
	{
	}

	public virtual void Add(string qualifierName, object qualifierValue)
	{
	}

	public virtual void Add(string qualifierName, object qualifierValue, bool isAmended, bool propagatesToInstance, bool propagatesToSubclass, bool isOverridable)
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(QualifierData[] qualifierArray, int index)
	{
	}

	public QualifierDataEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual void Remove(string qualifierName)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
