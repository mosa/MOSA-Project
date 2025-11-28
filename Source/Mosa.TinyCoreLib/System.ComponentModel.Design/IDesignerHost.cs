namespace System.ComponentModel.Design;

public interface IDesignerHost : IServiceContainer, IServiceProvider
{
	IContainer Container { get; }

	bool InTransaction { get; }

	bool Loading { get; }

	IComponent RootComponent { get; }

	string RootComponentClassName { get; }

	string TransactionDescription { get; }

	event EventHandler Activated;

	event EventHandler Deactivated;

	event EventHandler LoadComplete;

	event DesignerTransactionCloseEventHandler TransactionClosed;

	event DesignerTransactionCloseEventHandler TransactionClosing;

	event EventHandler TransactionOpened;

	event EventHandler TransactionOpening;

	void Activate();

	IComponent CreateComponent(Type componentClass);

	IComponent CreateComponent(Type componentClass, string name);

	DesignerTransaction CreateTransaction();

	DesignerTransaction CreateTransaction(string description);

	void DestroyComponent(IComponent component);

	IDesigner? GetDesigner(IComponent component);

	Type? GetType(string typeName);
}
