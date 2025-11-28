using System.Collections;

namespace System.Management;

public class ManagementObjectCollection : ICollection, IEnumerable, IDisposable
{
	public class ManagementObjectEnumerator : IEnumerator, IDisposable
	{
		public ManagementBaseObject Current
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

		internal ManagementObjectEnumerator()
		{
		}

		public void Dispose()
		{
		}

		~ManagementObjectEnumerator()
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

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	internal ManagementObjectCollection()
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(ManagementBaseObject[] objectCollection, int index)
	{
	}

	public void Dispose()
	{
	}

	~ManagementObjectCollection()
	{
	}

	public ManagementObjectEnumerator GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
