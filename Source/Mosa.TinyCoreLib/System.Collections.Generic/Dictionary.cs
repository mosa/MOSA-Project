using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public class Dictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary, IDeserializationCallback, ISerializable where TKey : notnull
{
	public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable, IDictionaryEnumerator
	{
		private object _dummy;

		private int _dummyPrimitive;

		public KeyValuePair<TKey, TValue> Current
		{
			get
			{
				throw null;
			}
		}

		DictionaryEntry IDictionaryEnumerator.Entry
		{
			get
			{
				throw null;
			}
		}

		object IDictionaryEnumerator.Key
		{
			get
			{
				throw null;
			}
		}

		object? IDictionaryEnumerator.Value
		{
			get
			{
				throw null;
			}
		}

		object? IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}
	}

	public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, IReadOnlyCollection<TKey>, ICollection
	{
		public struct Enumerator : IEnumerator<TKey>, IEnumerator, IDisposable
		{
			private TKey _currentKey;

			private object _dummy;

			private int _dummyPrimitive;

			public TKey Current
			{
				get
				{
					throw null;
				}
			}

			object? IEnumerator.Current
			{
				get
				{
					throw null;
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				throw null;
			}

			void IEnumerator.Reset()
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

		public KeyCollection(Dictionary<TKey, TValue> dictionary)
		{
		}

		public void CopyTo(TKey[] array, int index)
		{
		}

		public Enumerator GetEnumerator()
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

		IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
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
		public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			private TValue _currentValue;

			private object _dummy;

			private int _dummyPrimitive;

			public TValue Current
			{
				get
				{
					throw null;
				}
			}

			object? IEnumerator.Current
			{
				get
				{
					throw null;
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				throw null;
			}

			void IEnumerator.Reset()
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

		public ValueCollection(Dictionary<TKey, TValue> dictionary)
		{
		}

		public void CopyTo(TValue[] array, int index)
		{
		}

		public Enumerator GetEnumerator()
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

		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
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

	public IEqualityComparer<TKey> Comparer
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

	public TValue this[TKey key]
	{
		get
		{
			throw null;
		}
		set
		{
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

	public Dictionary()
	{
	}

	public Dictionary(IDictionary<TKey, TValue> dictionary)
	{
	}

	public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer)
	{
	}

	public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
	{
	}

	public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
	{
	}

	public Dictionary(IEqualityComparer<TKey>? comparer)
	{
	}

	public Dictionary(int capacity)
	{
	}

	public Dictionary(int capacity, IEqualityComparer<TKey>? comparer)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Dictionary(SerializationInfo info, StreamingContext context)
	{
	}

	public void Add(TKey key, TValue value)
	{
	}

	public void Clear()
	{
	}

	public bool ContainsKey(TKey key)
	{
		throw null;
	}

	public bool ContainsValue(TValue value)
	{
		throw null;
	}

	public int EnsureCapacity(int capacity)
	{
		throw null;
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public virtual void OnDeserialization(object? sender)
	{
	}

	public bool Remove(TKey key)
	{
		throw null;
	}

	public bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
	{
		throw null;
	}

	void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
	{
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	void IDictionary.Add(object key, object? value)
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

	public void TrimExcess()
	{
	}

	public void TrimExcess(int capacity)
	{
	}

	public bool TryAdd(TKey key, TValue value)
	{
		throw null;
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
	{
		throw null;
	}
}
