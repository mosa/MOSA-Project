using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public class LinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable, IDeserializationCallback, ISerializable
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

		void IDeserializationCallback.OnDeserialization(object? sender)
		{
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
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

	public LinkedListNode<T>? First
	{
		get
		{
			throw null;
		}
	}

	public LinkedListNode<T>? Last
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

	public LinkedList()
	{
	}

	public LinkedList(IEnumerable<T> collection)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected LinkedList(SerializationInfo info, StreamingContext context)
	{
	}

	public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
	{
	}

	public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
	{
		throw null;
	}

	public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
	{
	}

	public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
	{
		throw null;
	}

	public void AddFirst(LinkedListNode<T> node)
	{
	}

	public LinkedListNode<T> AddFirst(T value)
	{
		throw null;
	}

	public void AddLast(LinkedListNode<T> node)
	{
	}

	public LinkedListNode<T> AddLast(T value)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(T value)
	{
		throw null;
	}

	public void CopyTo(T[] array, int index)
	{
	}

	public LinkedListNode<T>? Find(T value)
	{
		throw null;
	}

	public LinkedListNode<T>? FindLast(T value)
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

	public void Remove(LinkedListNode<T> node)
	{
	}

	public bool Remove(T value)
	{
		throw null;
	}

	public void RemoveFirst()
	{
	}

	public void RemoveLast()
	{
	}

	void ICollection<T>.Add(T value)
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
}
