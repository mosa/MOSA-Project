using System.Collections;

namespace System.Management;

public class PropertyDataCollection : ICollection, IEnumerable
{
	public class PropertyDataEnumerator : IEnumerator
	{
		public PropertyData Current
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

		internal PropertyDataEnumerator()
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

	public virtual PropertyData this[string propertyName]
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

	internal PropertyDataCollection()
	{
	}

	public void Add(string propertyName, CimType propertyType, bool isArray)
	{
	}

	public virtual void Add(string propertyName, object propertyValue)
	{
	}

	public void Add(string propertyName, object propertyValue, CimType propertyType)
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(PropertyData[] propertyArray, int index)
	{
	}

	public PropertyDataEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual void Remove(string propertyName)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
