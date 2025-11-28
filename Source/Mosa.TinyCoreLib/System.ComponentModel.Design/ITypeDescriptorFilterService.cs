using System.Collections;

namespace System.ComponentModel.Design;

public interface ITypeDescriptorFilterService
{
	bool FilterAttributes(IComponent component, IDictionary attributes);

	bool FilterEvents(IComponent component, IDictionary events);

	bool FilterProperties(IComponent component, IDictionary properties);
}
