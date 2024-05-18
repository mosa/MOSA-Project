namespace System.ComponentModel.Design;

public interface IDesignerEventService
{
	IDesignerHost? ActiveDesigner { get; }

	DesignerCollection Designers { get; }

	event ActiveDesignerEventHandler ActiveDesignerChanged;

	event DesignerEventHandler DesignerCreated;

	event DesignerEventHandler DesignerDisposed;

	event EventHandler SelectionChanged;
}
