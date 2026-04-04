using System.Collections.Generic;

namespace System.Collections;

public class Stack : ICollection, IEnumerable, ICloneable
{
	private class SynchronizedStack(Stack backingStack) : Stack
	{
		public override int Count => backingStack.Count;

		public override bool IsSynchronized => true;

		public override object SyncRoot => backingStack.SyncRoot;

		public override void Clear()
		{
			lock (SyncRoot)
				backingStack.Clear();
		}

		public override object Clone()
		{
			lock (SyncRoot)
				return backingStack.Clone();
		}

		public override bool Contains(object obj)
		{
			lock (SyncRoot)
				return backingStack.Contains(obj);
		}

		public override void CopyTo(Array array, int index)
		{
			lock (SyncRoot)
				backingStack.CopyTo(array, index);
		}

		public override IEnumerator GetEnumerator()
		{
			lock (SyncRoot)
				return backingStack.GetEnumerator();
		}

		public override object Peek()
		{
			lock (SyncRoot)
				return backingStack.Peek();
		}

		public override object Pop()
		{
			lock (SyncRoot)
				return backingStack.Pop();
		}

		public override void Push(object obj)
		{
			lock (SyncRoot)
				backingStack.Push(obj);
		}

		public override object[] ToArray()
		{
			lock (SyncRoot)
				return backingStack.ToArray();
		}
	}

	public virtual int Count => backingList.Count;

	public virtual bool IsSynchronized => false;

	public virtual object SyncRoot => this;

	private readonly LinkedList<object?> backingList;

	public Stack() => backingList = [];

	public Stack(ICollection col)
	{
		ArgumentNullException.ThrowIfNull(col);

		backingList = [];

		// FIXME: The official implementation also uses Push() here, even
		// though it's a virtual method... so I'm assuming it's okay to use it??
		foreach (var item in col)
			Push(item);
	}

	public Stack(int initialCapacity)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(initialCapacity, nameof(initialCapacity));

		backingList = [];
	}

	public virtual void Clear() => backingList.Clear();

	public virtual object Clone()
	{
		var newStack = new Stack(Count);
		var current = backingList.Last;

		// Since Clone() just creates a copy of the Stack, the items must keep
		// the same order
		while (current is not null)
		{
			newStack.Push(current.Value);
			current = current.Previous;
		}

		return newStack;
	}

	public virtual bool Contains(object? obj) => backingList.Contains(obj);

	public virtual void CopyTo(Array array, int index)
	{
		if (Count > array.Length - index)
			Internal.Exceptions.Stack.ThrowStackTooBigException(nameof(Count));

		(backingList as ICollection).CopyTo(array, index);
	}

	public virtual IEnumerator GetEnumerator() => backingList.GetEnumerator();

	public virtual object? Peek()
	{
		if (Count == 0)
			Internal.Exceptions.Stack.ThrowStackEmptyException();

		return backingList.First!.Value;
	}

	public virtual object? Pop()
	{
		if (Count == 0)
			Internal.Exceptions.Stack.ThrowStackEmptyException();

		var value = backingList.First!.Value;
		backingList.RemoveFirst();
		return value;
	}

	public virtual void Push(object? obj) => backingList.AddFirst(obj);

	public static Stack Synchronized(Stack stack) => new SynchronizedStack(stack);

	public virtual object?[] ToArray()
	{
		var newList = new List<object?>();

		while (Count != 0)
			newList.Add(Pop());

		return newList.ToArray();
	}
}
