using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public class SortedSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, IReadOnlySet<T>, ICollection, IDeserializationCallback, ISerializable
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable, IDeserializationCallback, ISerializable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public T Current
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

		void IDeserializationCallback.OnDeserialization(object? sender)
		{
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
		}
	}

	public IComparer<T> Comparer
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

	public T? Max
	{
		get
		{
			throw null;
		}
	}

	public T? Min
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<T>.IsReadOnly
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

	public SortedSet()
	{
	}

	public SortedSet(IComparer<T>? comparer)
	{
	}

	public SortedSet(IEnumerable<T> collection)
	{
	}

	public SortedSet(IEnumerable<T> collection, IComparer<T>? comparer)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SortedSet(SerializationInfo info, StreamingContext context)
	{
	}

	public bool Add(T item)
	{
		throw null;
	}

	public virtual void Clear()
	{
	}

	public virtual bool Contains(T item)
	{
		throw null;
	}

	public void CopyTo(T[] array)
	{
	}

	public void CopyTo(T[] array, int index)
	{
	}

	public void CopyTo(T[] array, int index, int count)
	{
	}

	public static IEqualityComparer<SortedSet<T>> CreateSetComparer()
	{
		throw null;
	}

	public static IEqualityComparer<SortedSet<T>> CreateSetComparer(IEqualityComparer<T>? memberEqualityComparer)
	{
		throw null;
	}

	public void ExceptWith(IEnumerable<T> other)
	{
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public virtual SortedSet<T> GetViewBetween(T? lowerValue, T? upperValue)
	{
		throw null;
	}

	public virtual void IntersectWith(IEnumerable<T> other)
	{
	}

	public bool IsProperSubsetOf(IEnumerable<T> other)
	{
		throw null;
	}

	public bool IsProperSupersetOf(IEnumerable<T> other)
	{
		throw null;
	}

	public bool IsSubsetOf(IEnumerable<T> other)
	{
		throw null;
	}

	public bool IsSupersetOf(IEnumerable<T> other)
	{
		throw null;
	}

	protected virtual void OnDeserialization(object? sender)
	{
	}

	public bool Overlaps(IEnumerable<T> other)
	{
		throw null;
	}

	public bool Remove(T item)
	{
		throw null;
	}

	public int RemoveWhere(Predicate<T> match)
	{
		throw null;
	}

	public IEnumerable<T> Reverse()
	{
		throw null;
	}

	public bool SetEquals(IEnumerable<T> other)
	{
		throw null;
	}

	public void SymmetricExceptWith(IEnumerable<T> other)
	{
	}

	void ICollection<T>.Add(T item)
	{
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
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

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public bool TryGetValue(T equalValue, [MaybeNullWhen(false)] out T actualValue)
	{
		throw null;
	}

	public void UnionWith(IEnumerable<T> other)
	{
	}
}
