using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public interface IContainer : IDisposable
{
	ComponentCollection Components { get; }

	void Add(IComponent? component);

	[RequiresUnreferencedCode("The Type of components in the container cannot be statically discovered to validate the name.")]
	void Add(IComponent? component, string? name);

	void Remove(IComponent? component);
}
