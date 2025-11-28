using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public class BindingList<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T> : Collection<T>, ICollection, IEnumerable, IList, IBindingList, ICancelAddNew, IRaiseItemChangedEvents
{
	public bool AllowEdit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AllowNew
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AllowRemove
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected virtual bool IsSortedCore
	{
		get
		{
			throw null;
		}
	}

	public bool RaiseListChangedEvents
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected virtual ListSortDirection SortDirectionCore
	{
		get
		{
			throw null;
		}
	}

	protected virtual PropertyDescriptor? SortPropertyCore
	{
		get
		{
			throw null;
		}
	}

	protected virtual bool SupportsChangeNotificationCore
	{
		get
		{
			throw null;
		}
	}

	protected virtual bool SupportsSearchingCore
	{
		get
		{
			throw null;
		}
	}

	protected virtual bool SupportsSortingCore
	{
		get
		{
			throw null;
		}
	}

	bool IBindingList.AllowEdit
	{
		get
		{
			throw null;
		}
	}

	bool IBindingList.AllowNew
	{
		get
		{
			throw null;
		}
	}

	bool IBindingList.AllowRemove
	{
		get
		{
			throw null;
		}
	}

	bool IBindingList.IsSorted
	{
		get
		{
			throw null;
		}
	}

	ListSortDirection IBindingList.SortDirection
	{
		get
		{
			throw null;
		}
	}

	PropertyDescriptor? IBindingList.SortProperty
	{
		get
		{
			throw null;
		}
	}

	bool IBindingList.SupportsChangeNotification
	{
		get
		{
			throw null;
		}
	}

	bool IBindingList.SupportsSearching
	{
		get
		{
			throw null;
		}
	}

	bool IBindingList.SupportsSorting
	{
		get
		{
			throw null;
		}
	}

	bool IRaiseItemChangedEvents.RaisesItemChangedEvents
	{
		get
		{
			throw null;
		}
	}

	public event AddingNewEventHandler AddingNew
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ListChangedEventHandler ListChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	[RequiresUnreferencedCode("Raises ListChanged events with PropertyDescriptors. PropertyDescriptors require unreferenced code.")]
	public BindingList()
	{
	}

	[RequiresUnreferencedCode("Raises ListChanged events with PropertyDescriptors. PropertyDescriptors require unreferenced code.")]
	public BindingList(IList<T> list)
	{
	}

	public T AddNew()
	{
		throw null;
	}

	protected virtual object? AddNewCore()
	{
		throw null;
	}

	protected virtual void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
	{
	}

	public virtual void CancelNew(int itemIndex)
	{
	}

	protected override void ClearItems()
	{
	}

	public virtual void EndNew(int itemIndex)
	{
	}

	protected virtual int FindCore(PropertyDescriptor prop, object key)
	{
		throw null;
	}

	protected override void InsertItem(int index, T item)
	{
	}

	protected virtual void OnAddingNew(AddingNewEventArgs e)
	{
	}

	protected virtual void OnListChanged(ListChangedEventArgs e)
	{
	}

	protected override void RemoveItem(int index)
	{
	}

	protected virtual void RemoveSortCore()
	{
	}

	public void ResetBindings()
	{
	}

	public void ResetItem(int position)
	{
	}

	protected override void SetItem(int index, T item)
	{
	}

	void IBindingList.AddIndex(PropertyDescriptor prop)
	{
	}

	object IBindingList.AddNew()
	{
		throw null;
	}

	void IBindingList.ApplySort(PropertyDescriptor prop, ListSortDirection direction)
	{
	}

	int IBindingList.Find(PropertyDescriptor prop, object key)
	{
		throw null;
	}

	void IBindingList.RemoveIndex(PropertyDescriptor prop)
	{
	}

	void IBindingList.RemoveSort()
	{
	}
}
