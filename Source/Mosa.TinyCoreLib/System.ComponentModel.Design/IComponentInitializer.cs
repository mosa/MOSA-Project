using System.Collections;

namespace System.ComponentModel.Design;

public interface IComponentInitializer
{
	void InitializeExistingComponent(IDictionary? defaultValues);

	void InitializeNewComponent(IDictionary? defaultValues);
}
