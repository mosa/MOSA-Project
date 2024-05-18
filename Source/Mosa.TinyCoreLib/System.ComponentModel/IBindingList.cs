using System.Collections;

namespace System.ComponentModel;

public interface IBindingList : ICollection, IEnumerable, IList
{
	bool AllowEdit { get; }

	bool AllowNew { get; }

	bool AllowRemove { get; }

	bool IsSorted { get; }

	ListSortDirection SortDirection { get; }

	PropertyDescriptor? SortProperty { get; }

	bool SupportsChangeNotification { get; }

	bool SupportsSearching { get; }

	bool SupportsSorting { get; }

	event ListChangedEventHandler ListChanged;

	void AddIndex(PropertyDescriptor property);

	object? AddNew();

	void ApplySort(PropertyDescriptor property, ListSortDirection direction);

	int Find(PropertyDescriptor property, object key);

	void RemoveIndex(PropertyDescriptor property);

	void RemoveSort();
}
