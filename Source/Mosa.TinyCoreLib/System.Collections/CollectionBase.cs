namespace System.Collections;

public abstract class CollectionBase : ICollection, IEnumerable, IList
{
	public int Capacity
	{
		get => backingList.Capacity;
		set => backingList.Capacity = value;
	}

	public int Count => backingList.Count;

	protected ArrayList InnerList => backingList;

	protected IList List => this;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	bool IList.IsFixedSize => false;

	bool IList.IsReadOnly => false;

	object? IList.this[int index]
	{
		get => backingList[index];
		set
		{
			var oldValue = backingList[index];

			OnValidate(value);
			OnSet(index, oldValue, value);
			backingList[index] = value;
			OnSetComplete(index, oldValue, value);
		}
	}

	private readonly ArrayList backingList;

	protected CollectionBase() => backingList = new ArrayList(Internal.Impl.CollectionBase.InitialArraySize);

	protected CollectionBase(int capacity) => backingList = new ArrayList(capacity);

	public void Clear()
	{
		OnClear();
		backingList.Clear();
		OnClearComplete();
	}

	public IEnumerator GetEnumerator() => backingList.GetEnumerator();

	protected virtual void OnClear() { }

	protected virtual void OnClearComplete() { }

	protected virtual void OnInsert(int index, object? value) { }

	protected virtual void OnInsertComplete(int index, object? value) { }

	protected virtual void OnRemove(int index, object? value) { }

	protected virtual void OnRemoveComplete(int index, object? value) { }

	protected virtual void OnSet(int index, object? oldValue, object? newValue) { }

	protected virtual void OnSetComplete(int index, object? oldValue, object? newValue) { }

	protected virtual void OnValidate(object value) => ArgumentNullException.ThrowIfNull(value);

	public void RemoveAt(int index)
	{
		var value = backingList[index];

		OnValidate(value);
		OnRemove(index, value);
		backingList.RemoveAt(index);
		OnRemoveComplete(index, value);
	}

	void ICollection.CopyTo(Array array, int index) => backingList.CopyTo(array, index);

	int IList.Add(object? value)
	{
		if (List.IsReadOnly || List.IsFixedSize)
			Internal.Exceptions.Generic.NotSupported();

		var index = Count - 1;

		OnValidate(value);
		OnInsert(index, value);
		backingList.Add(value);
		OnInsertComplete(index, value);

		return index;
	}

	bool IList.Contains(object? value) => backingList.Contains(value);

	int IList.IndexOf(object? value) => backingList.IndexOf(value);

	void IList.Insert(int index, object? value)
	{
		if (List.IsReadOnly || List.IsFixedSize)
			Internal.Exceptions.Generic.NotSupported();

		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count, nameof(index));

		OnValidate(value);
		OnInsert(index, value);
		backingList.Insert(index, value);
		OnInsertComplete(index, value);
	}

	void IList.Remove(object? value)
	{
		if (List.IsReadOnly || List.IsFixedSize)
			Internal.Exceptions.Generic.NotSupported();

		var index = backingList.IndexOf(value);
		if (index == -1)
			Internal.Exceptions.CollectionBase.ThrowValueNotFoundException();

		OnValidate(value);
		OnRemove(index, value);
		backingList.RemoveAt(index);
		OnRemoveComplete(index, value);
	}
}
