namespace System.ComponentModel.Design;

public interface IComponentChangeService
{
	event ComponentEventHandler ComponentAdded;

	event ComponentEventHandler ComponentAdding;

	event ComponentChangedEventHandler ComponentChanged;

	event ComponentChangingEventHandler ComponentChanging;

	event ComponentEventHandler ComponentRemoved;

	event ComponentEventHandler ComponentRemoving;

	event ComponentRenameEventHandler ComponentRename;

	void OnComponentChanged(object component, MemberDescriptor? member, object? oldValue, object? newValue);

	void OnComponentChanging(object component, MemberDescriptor? member);
}
