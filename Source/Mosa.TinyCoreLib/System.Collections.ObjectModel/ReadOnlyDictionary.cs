using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.ObjectModel;

public class ReadOnlyDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary where TKey : notnull
{
	public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, IReadOnlyCollection<TKey>, ICollection
	{
		public int Count
		{
			get
			{
				throw null;
			}
		}

		bool ICollection<TKey>.IsReadOnly
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

		internal KeyCollection()
		{
		}

		public void CopyTo(TKey[] array, int arrayIndex)
		{
		}

		public IEnumerator<TKey> GetEnumerator()
		{
			throw null;
		}

		void ICollection<TKey>.Add(TKey item)
		{
		}

		void ICollection<TKey>.Clear()
		{
		}

		public bool Contains(TKey item)
		{
			throw null;
		}

		bool ICollection<TKey>.Remove(TKey item)
		{
			throw null;
		}

		void ICollection.CopyTo(Array array, int index)
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, IReadOnlyCollection<TValue>, ICollection
	{
		public int Count
		{
			get
			{
				throw null;
			}
		}

		bool ICollection<TValue>.IsReadOnly
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

		internal ValueCollection()
		{
		}

		public void CopyTo(TValue[] array, int arrayIndex)
		{
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			throw null;
		}

		void ICollection<TValue>.Add(TValue item)
		{
		}

		void ICollection<TValue>.Clear()
		{
		}

		bool ICollection<TValue>.Contains(TValue item)
		{
			throw null;
		}

		bool ICollection<TValue>.Remove(TValue item)
		{
			throw null;
		}

		void ICollection.CopyTo(Array array, int index)
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
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

	protected IDictionary<TKey, TValue> Dictionary
	{
		get
		{
			throw null;
		}
	}

	public static ReadOnlyDictionary<TKey, TValue> Empty
	{
		get
		{
			throw null;
		}
	}

	public TValue this[TKey key]
	{
		get
		{
			throw null;
		}
	}

	public KeyCollection Keys
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	TValue IDictionary<TKey, TValue>.this[TKey key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	ICollection<TKey> IDictionary<TKey, TValue>.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection<TValue> IDictionary<TKey, TValue>.Values
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
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

	bool IDictionary.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IDictionary.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IDictionary.this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	ICollection IDictionary.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection IDictionary.Values
	{
		get
		{
			throw null;
		}
	}

	public ValueCollection Values
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
	{
	}

	public bool ContainsKey(TKey key)
	{
		throw null;
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Clear()
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
	{
		throw null;
	}

	void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
	{
	}

	bool IDictionary<TKey, TValue>.Remove(TKey key)
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	void IDictionary.Add(object key, object? value)
	{
	}

	void IDictionary.Clear()
	{
	}

	bool IDictionary.Contains(object key)
	{
		throw null;
	}

	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		throw null;
	}

	void IDictionary.Remove(object key)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}
}
