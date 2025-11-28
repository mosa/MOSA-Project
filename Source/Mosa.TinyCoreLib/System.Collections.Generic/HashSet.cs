using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public class HashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, IReadOnlySet<T>, IDeserializationCallback, ISerializable
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private T _current;

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
	}

	public IEqualityComparer<T> Comparer
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

	bool ICollection<T>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public HashSet()
	{
	}

	public HashSet(IEnumerable<T> collection)
	{
	}

	public HashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer)
	{
	}

	public HashSet(IEqualityComparer<T>? comparer)
	{
	}

	public HashSet(int capacity)
	{
	}

	public HashSet(int capacity, IEqualityComparer<T>? comparer)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected HashSet(SerializationInfo info, StreamingContext context)
	{
	}

	public bool Add(T item)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(T item)
	{
		throw null;
	}

	public void CopyTo(T[] array)
	{
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
	}

	public void CopyTo(T[] array, int arrayIndex, int count)
	{
	}

	public static IEqualityComparer<HashSet<T>> CreateSetComparer()
	{
		throw null;
	}

	public int EnsureCapacity(int capacity)
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

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public void IntersectWith(IEnumerable<T> other)
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

	public virtual void OnDeserialization(object? sender)
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

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public void TrimExcess()
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
