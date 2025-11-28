using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

public class ActivityTagsCollection : IDictionary<string, object?>, ICollection<KeyValuePair<string, object?>>, IEnumerable<KeyValuePair<string, object?>>, IEnumerable
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Enumerator : IEnumerator<KeyValuePair<string, object?>>, IEnumerator, IDisposable
	{
		public KeyValuePair<string, object?> Current
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

		public void Dispose()
		{
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public object? this[string key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICollection<string> Keys
	{
		get
		{
			throw null;
		}
	}

	public ICollection<object?> Values
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public ActivityTagsCollection()
	{
		throw null;
	}

	public ActivityTagsCollection(IEnumerable<KeyValuePair<string, object?>> list)
	{
		throw null;
	}

	public void Add(string key, object? value)
	{
		throw null;
	}

	public void Add(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	public void Clear()
	{
		throw null;
	}

	public bool Contains(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	public bool ContainsKey(string key)
	{
		throw null;
	}

	public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
	{
		throw null;
	}

	public bool Remove(string key)
	{
		throw null;
	}

	public bool Remove(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	public bool TryGetValue(string key, out object? value)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}
}
