using System.Collections;

namespace System.Management;

public class MethodDataCollection : ICollection, IEnumerable
{
	public class MethodDataEnumerator : IEnumerator
	{
		public MethodData Current
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

		internal MethodDataEnumerator()
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

	public virtual MethodData this[string methodName]
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

	internal MethodDataCollection()
	{
	}

	public virtual void Add(string methodName)
	{
	}

	public virtual void Add(string methodName, ManagementBaseObject inParameters, ManagementBaseObject outParameters)
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(MethodData[] methodArray, int index)
	{
	}

	public MethodDataEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual void Remove(string methodName)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
