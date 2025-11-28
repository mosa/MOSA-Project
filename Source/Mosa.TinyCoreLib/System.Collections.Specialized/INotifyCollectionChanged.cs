namespace System.Collections.Specialized;

public interface INotifyCollectionChanged
{
	event NotifyCollectionChangedEventHandler? CollectionChanged;
}
