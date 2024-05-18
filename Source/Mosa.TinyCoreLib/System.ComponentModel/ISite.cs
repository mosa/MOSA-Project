using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public interface ISite : IServiceProvider
{
	IComponent Component { get; }

	IContainer? Container { get; }

	bool DesignMode { get; }

	string? Name
	{
		get; [RequiresUnreferencedCode("The Type of components in the container cannot be statically discovered to validate the name.")]
		set;
	}
}
