using System.Collections;

namespace System.ComponentModel.Design.Serialization;

public interface IDesignerLoaderService
{
	void AddLoadDependency();

	void DependentLoadComplete(bool successful, ICollection? errorCollection);

	bool Reload();
}
