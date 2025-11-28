using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Collections.ObjectModel;

public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
	public virtual event NotifyCollectionChangedEventHandler? CollectionChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	protected virtual event PropertyChangedEventHandler? PropertyChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public ObservableCollection()
	{
	}

	public ObservableCollection(IEnumerable<T> collection)
	{
	}

	public ObservableCollection(List<T> list)
	{
	}

	protected IDisposable BlockReentrancy()
	{
		throw null;
	}

	protected void CheckReentrancy()
	{
	}

	protected override void ClearItems()
	{
	}

	protected override void InsertItem(int index, T item)
	{
	}

	public void Move(int oldIndex, int newIndex)
	{
	}

	protected virtual void MoveItem(int oldIndex, int newIndex)
	{
	}

	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
	}

	protected override void RemoveItem(int index)
	{
	}

	protected override void SetItem(int index, T item)
	{
	}
}
