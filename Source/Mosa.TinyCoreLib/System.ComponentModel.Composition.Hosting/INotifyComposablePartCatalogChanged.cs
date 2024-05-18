namespace System.ComponentModel.Composition.Hosting;

public interface INotifyComposablePartCatalogChanged
{
	event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed;

	event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing;
}
