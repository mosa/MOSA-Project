using System.Collections;

namespace System.ComponentModel.Design.Serialization;

public interface IDesignerLoaderHost : IDesignerHost, IServiceContainer, IServiceProvider
{
	void EndLoad(string baseClassName, bool successful, ICollection? errorCollection);

	void Reload();
}
