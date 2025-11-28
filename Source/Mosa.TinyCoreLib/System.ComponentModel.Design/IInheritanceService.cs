namespace System.ComponentModel.Design;

public interface IInheritanceService
{
	void AddInheritedComponents(IComponent component, IContainer container);

	InheritanceAttribute GetInheritanceAttribute(IComponent component);
}
