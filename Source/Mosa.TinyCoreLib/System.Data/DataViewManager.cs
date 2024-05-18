using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[Designer("Microsoft.VSDesigner.Data.VS.DataViewManagerDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class DataViewManager : MarshalByValueComponent, ICollection, IEnumerable, IList, IBindingList, ITypedList
{
	[DefaultValue(null)]
	public DataSet? DataSet
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public string DataViewSettingCollectionString
	{
		get
		{
			throw null;
		}
		[RequiresUnreferencedCode("Members of types used in the RowFilter expression might be trimmed.")]
		set
		{
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataViewSettingCollection DataViewSettings
	{
		get
		{
			throw null;
		}
	}

	int ICollection.Count
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

	bool IList.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IList.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IList.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
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

	public event ListChangedEventHandler? ListChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public DataViewManager()
	{
	}

	public DataViewManager(DataSet? dataSet)
	{
	}

	public DataView CreateDataView(DataTable table)
	{
		throw null;
	}

	protected virtual void OnListChanged(ListChangedEventArgs e)
	{
	}

	protected virtual void RelationCollectionChanged(object sender, CollectionChangeEventArgs e)
	{
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	int IList.Add(object? value)
	{
		throw null;
	}

	void IList.Clear()
	{
	}

	bool IList.Contains(object? value)
	{
		throw null;
	}

	int IList.IndexOf(object? value)
	{
		throw null;
	}

	void IList.Insert(int index, object? value)
	{
	}

	void IList.Remove(object? value)
	{
	}

	void IList.RemoveAt(int index)
	{
	}

	void IBindingList.AddIndex(PropertyDescriptor property)
	{
	}

	object? IBindingList.AddNew()
	{
		throw null;
	}

	void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
	{
	}

	int IBindingList.Find(PropertyDescriptor property, object key)
	{
		throw null;
	}

	void IBindingList.RemoveIndex(PropertyDescriptor property)
	{
	}

	void IBindingList.RemoveSort()
	{
	}

	PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[]? listAccessors)
	{
		throw null;
	}

	string ITypedList.GetListName(PropertyDescriptor[]? listAccessors)
	{
		throw null;
	}

	protected virtual void TableCollectionChanged(object sender, CollectionChangeEventArgs e)
	{
	}
}
