using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.ObjectModel;

public abstract class KeyedCollection<TKey, TItem> : Collection<TItem> where TKey : notnull
{
	public IEqualityComparer<TKey> Comparer
	{
		get
		{
			throw null;
		}
	}

	protected IDictionary<TKey, TItem>? Dictionary
	{
		get
		{
			throw null;
		}
	}

	public TItem this[TKey key]
	{
		get
		{
			throw null;
		}
	}

	protected KeyedCollection()
	{
	}

	protected KeyedCollection(IEqualityComparer<TKey>? comparer)
	{
	}

	protected KeyedCollection(IEqualityComparer<TKey>? comparer, int dictionaryCreationThreshold)
	{
	}

	protected void ChangeItemKey(TItem item, TKey newKey)
	{
	}

	protected override void ClearItems()
	{
	}

	public bool Contains(TKey key)
	{
		throw null;
	}

	protected abstract TKey GetKeyForItem(TItem item);

	protected override void InsertItem(int index, TItem item)
	{
	}

	public bool Remove(TKey key)
	{
		throw null;
	}

	protected override void RemoveItem(int index)
	{
	}

	protected override void SetItem(int index, TItem item)
	{
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TItem item)
	{
		throw null;
	}
}
