using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public class LinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable, IDeserializationCallback, ISerializable
	{
		public T Current => current.nodeValue;

		object? IEnumerator.Current => current.nodeValue;

		private LinkedListNode<T> current;
		private readonly LinkedList<T> list;

		internal Enumerator(LinkedList<T> instance) => list = instance;

		public void Dispose()
		{
			list.enumerating = false;
			list.modified = false;
		}

		// Implementation detail: Current stays at the last value when the enumerator passes the end of the list
		public bool MoveNext()
		{
			if (!list.enumerating || list.modified)
				Internal.Exceptions.LinkedList.Enumerator.ThrowListModifiedException();

			if (current is null && list.first is not null)
			{
				current = list.first;
				return true;
			}

			if (current is not null && current.next is not null)
			{
				current = current.next;
				return true;
			}

			return false;
		}

		void IEnumerator.Reset()
		{
			if (!list.enumerating || list.modified)
				Internal.Exceptions.LinkedList.Enumerator.ThrowListModifiedException();

			current = null;
		}

		void IDeserializationCallback.OnDeserialization(object? sender) { }

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) { }
	}

	public int Count => count;

	public LinkedListNode<T>? First => first;

	public LinkedListNode<T>? Last => last;

	bool ICollection<T>.IsReadOnly => false;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	private int count;
	private LinkedListNode<T>? first, last;
	private bool enumerating, modified;

	public LinkedList() { }

	public LinkedList(IEnumerable<T> collection)
	{
		foreach (var element in collection)
			AddLast(element);
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected LinkedList(SerializationInfo info, StreamingContext context) { }

	public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
	{
		ArgumentNullException.ThrowIfNull(node, nameof(node));
		ArgumentNullException.ThrowIfNull(newNode, nameof(newNode));

		if (node.list != this)
			Internal.Exceptions.LinkedList.ThrowNodeNotInListException();

		if (newNode.list is not null && newNode.list != this)
			Internal.Exceptions.LinkedList.ThrowNewNodeInAnotherListException();

		if (node == last)
			last = newNode;

		newNode.list = this;
		newNode.previous = node;
		newNode.next = node.next;
		node.next = newNode;

		count++;

		if (enumerating)
			modified = true;
	}

	public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
	{
		var newNode = new LinkedListNode<T>(value);
		AddAfter(node, newNode);
		return newNode;
	}

	public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
	{
		if (node.previous is null)
			AddFirst(newNode);
		else
			AddAfter(node.previous, newNode);
	}

	public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
	{
		var newNode = new LinkedListNode<T>(value);
		AddBefore(node, newNode);
		return newNode;
	}

	public void AddFirst(LinkedListNode<T> node)
	{
		ArgumentNullException.ThrowIfNull(node, nameof(node));

		if (node.list is not null && node.list != this)
			Internal.Exceptions.LinkedList.ThrowNewNodeInAnotherListException();

		if (first is not null)
			first.previous = node;

		if (count == 0)
			last = node;

		node.list = this;
		node.previous = null;
		node.next = first;

		first = node;
		count++;

		if (enumerating)
			modified = true;
	}

	public LinkedListNode<T> AddFirst(T value)
	{
		var node = new LinkedListNode<T>(value);
		AddFirst(node);
		return node;
	}

	public void AddLast(LinkedListNode<T> node)
	{
		ArgumentNullException.ThrowIfNull(node, nameof(node));

		if (node.list is not null && node.list != this)
			Internal.Exceptions.LinkedList.ThrowNewNodeInAnotherListException();

		if (last is not null)
			last.next = node;

		if (count == 0)
			first = node;

		node.list = this;
		node.previous = last;
		node.next = null;

		last = node;
		count++;

		if (enumerating)
			modified = true;
	}

	public LinkedListNode<T> AddLast(T value)
	{
		var node = new LinkedListNode<T>(value);
		AddLast(node);
		return node;
	}

	public void Clear()
	{
		var current = first;
		while (current is not null)
		{
			current.list = null;
			current.previous = null;

			var next = current.next;
			current.next = null;
			current = next;
		}

		count = 0;
		first = null;
		last = null;

		if (enumerating)
			modified = true;
	}

	public bool Contains(T value)
	{
		var current = first;
		while (current is not null)
		{
			if (EqualityComparer<T>.Default.Equals(current.nodeValue, value))
				return true;
			current = current.next;
		}

		return false;
	}

	public void CopyTo(T[] array, int index)
	{
		ArgumentNullException.ThrowIfNull(array, nameof(array));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));

		if (count > array.Length - index)
			Internal.Exceptions.LinkedList.ThrowListTooBigException(nameof(count));

		var current = first;
		var i = index;
		while (current is not null)
		{
			array[i++] = current.nodeValue;
			current = current.next;
		}
	}

	public LinkedListNode<T>? Find(T value)
	{
		var current = first;
		while (current is not null)
		{
			if (EqualityComparer<T>.Default.Equals(current.nodeValue, value))
				return current;
			current = current.next;
		}

		return null;
	}

	public LinkedListNode<T>? FindLast(T value)
	{
		var current = last;
		while (current is not null)
		{
			if (EqualityComparer<T>.Default.Equals(current.nodeValue, value))
				return current;
			current = current.previous;
		}

		return null;
	}

	public Enumerator GetEnumerator()
	{
		enumerating = true;
		return new(this);
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { }

	public virtual void OnDeserialization(object? sender) { }

	public void Remove(LinkedListNode<T> node)
	{
		ArgumentNullException.ThrowIfNull(node, nameof(node));

		if (node.list != this)
			Internal.Exceptions.LinkedList.ThrowNodeNotInListException();

		if (node.previous is not null)
			node.previous.next = null;
		if (node.next is not null)
			node.next.previous = null;

		node.list = null;
		node.previous = null;
		node.next = null;

		count--;

		if (enumerating)
			modified = true;
	}

	public bool Remove(T value)
	{
		var current = first;
		while (current is not null)
		{
			if (EqualityComparer<T>.Default.Equals(current.nodeValue, value))
			{
				Remove(current);
				return true;
			}
			current = current.next;
		}

		return false;
	}

	public void RemoveFirst()
	{
		if (first is null)
			Internal.Exceptions.LinkedList.ThrowListEmptyException(nameof(First));

		if (first.next is not null)
			first.next.previous = null;

		first.list = null;
		first.previous = null;
		var next = first.next;
		first.next = null;

		if (last == first)
			last = next;
		first = next;

		count--;

		if (enumerating)
			modified = true;
	}

	public void RemoveLast()
	{
		if (last is null)
			Internal.Exceptions.LinkedList.ThrowListEmptyException(nameof(Last));

		if (last.previous is not null)
			last.previous.next = null;

		last.list = null;
		var previous = last.previous;
		last.previous = null;
		last.next = null;

		if (first == last)
			first = previous;
		last = previous;

		count--;

		if (enumerating)
			modified = true;
	}

	void ICollection<T>.Add(T value) => AddLast(value);

	IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

	void ICollection.CopyTo(Array array, int index)
	{
		ArgumentNullException.ThrowIfNull(array, nameof(array));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));

		if (array.Rank != 1)
			Internal.Exceptions.Generic.ThrowMultiDimensionalArrayException(nameof(array));

		if (array.GetLowerBound(0) != 0)
			Internal.Exceptions.Generic.ThrowArrayNotZeroBasedIndexingException(nameof(array));

		if (array.GetType().GetElementType() is not T)
			Internal.Exceptions.Generic.ThrowArrayTypeMismatchException(nameof(array));

		if (count > array.Length - index)
			Internal.Exceptions.LinkedList.ThrowListTooBigException(nameof(count));

		var current = first;
		var i = index;
		while (current is not null)
		{
			array.SetValue(current.nodeValue, i++);
			current = current.next;
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
