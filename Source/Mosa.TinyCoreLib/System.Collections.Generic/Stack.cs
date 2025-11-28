using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public class Stack<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
{
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		public T Current => current.Value;

		object? IEnumerator.Current => current.Value;

		private LinkedListNode<T> current;
		private readonly Stack<T> stack;

		internal Enumerator(Stack<T> instance) => stack = instance;

		public void Dispose()
		{
			stack.enumerating = false;
			stack.modified = false;
		}

		// Implementation detail: Current stays at the last value when the enumerator passes the end of the stack
		public bool MoveNext()
		{
			if (!stack.enumerating || stack.modified)
			{
				Internal.Exceptions.Stack.Enumerator.ThrowStackModifiedException();
				return false;
			}

			if (current is null && stack.backingList.First is not null)
			{
				current = stack.backingList.First;
				return true;
			}

			if (current?.Next != null)
			{
				current = current.Next;
				return true;
			}

			return false;
		}

		void IEnumerator.Reset()
		{
			if (!stack.enumerating || stack.modified)
				Internal.Exceptions.Stack.Enumerator.ThrowStackModifiedException();

			current = null;
		}
	}

	public int Count => backingList.Count;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	private bool enumerating, modified;
	private readonly LinkedList<T> backingList;

	public Stack() => backingList = [];

	public Stack(IEnumerable<T> collection)
	{
		ArgumentNullException.ThrowIfNull(collection, nameof(collection));

		backingList = [];

		foreach (var item in collection)
			backingList.AddFirst(item);
	}

	// Implementation detail: a linked list doesn't require an initial capacity, so we don't use the parameter
	public Stack(int capacity)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));

		backingList = [];
	}

	public void Clear() => backingList.Clear();

	public bool Contains(T item) => backingList.Contains(item);

	// We're allowed to forward to the linked list's implementation of the method because the linked list
	// will iterate starting from the first element, and our Stack implementation adds from the first element
	// of the linked list
	// We only check if the stack is too big before so we can have a more personalized error message
	public void CopyTo(T[] array, int arrayIndex)
	{
		if (Count > array.Length - arrayIndex)
			Internal.Exceptions.Stack.ThrowStackTooBigException(nameof(Count));

		backingList.CopyTo(array, arrayIndex);
	}

	public Enumerator GetEnumerator()
	{
		enumerating = true;
		return new(this);
	}

	public T Peek()
	{
		if (Count == 0)
			Internal.Exceptions.Stack.ThrowStackEmptyException();

		return backingList.First!.Value;
	}

	public T Pop()
	{
		if (Count == 0)
			Internal.Exceptions.Stack.ThrowStackEmptyException();

		var value = backingList.First!.Value;
		backingList.RemoveFirst();

		if (enumerating)
			modified = true;

		return value;
	}

	public void Push(T item)
	{
		backingList.AddFirst(item);

		if (enumerating)
			modified = true;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

	// Same remark than with the generic CopyTo() method
	void ICollection.CopyTo(Array array, int arrayIndex)
	{
		if (Count > array.Length - arrayIndex)
			Internal.Exceptions.Stack.ThrowStackTooBigException(nameof(Count));

		(backingList as ICollection).CopyTo(array, arrayIndex);
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	// Implementation detail: we don't need to ensure a minimum capacity with a linked list, so
	// we just return the supposed minimum capacity to ensure
	public int EnsureCapacity(int capacity) => capacity;

	public T[] ToArray()
	{
		var newList = new List<T>();

		while (Count != 0)
			newList.Add(Pop());

		return newList.ToArray();
	}

	// Same remark than with other methods depending on the capacity...
	public void TrimExcess() { }

	public bool TryPeek([MaybeNullWhen(false)] out T result)
	{
		if (Count == 0)
		{
			result = default;
			return false;
		}

		result = Peek();

		if (enumerating)
			modified = true;

		return true;
	}

	public bool TryPop([MaybeNullWhen(false)] out T result)
	{
		if (Count == 0)
		{
			result = default;
			return false;
		}

		result = Pop();

		if (enumerating)
			modified = true;

		return true;
	}
}
