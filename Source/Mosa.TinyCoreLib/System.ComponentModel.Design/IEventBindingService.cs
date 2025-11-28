using System.Collections;

namespace System.ComponentModel.Design;

public interface IEventBindingService
{
	string CreateUniqueMethodName(IComponent component, EventDescriptor e);

	ICollection GetCompatibleMethods(EventDescriptor e);

	EventDescriptor? GetEvent(PropertyDescriptor property);

	PropertyDescriptorCollection GetEventProperties(EventDescriptorCollection events);

	PropertyDescriptor GetEventProperty(EventDescriptor e);

	bool ShowCode();

	bool ShowCode(IComponent component, EventDescriptor e);

	bool ShowCode(int lineNumber);
}
