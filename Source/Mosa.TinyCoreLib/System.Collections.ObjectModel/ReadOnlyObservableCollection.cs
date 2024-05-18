using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Collections.ObjectModel;

public class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
	public new static ReadOnlyObservableCollection<T> Empty
	{
		get
		{
			throw null;
		}
	}

	protected virtual event NotifyCollectionChangedEventHandler? CollectionChanged
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

	event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
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

	public ReadOnlyObservableCollection(ObservableCollection<T> list)
		: base((IList<T>)null)
	{
	}

	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
	{
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
	{
	}
}
