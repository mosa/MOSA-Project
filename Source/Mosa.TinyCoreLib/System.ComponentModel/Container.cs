using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public class Container : IContainer, IDisposable
{
	public virtual ComponentCollection Components
	{
		get
		{
			throw null;
		}
	}

	public virtual void Add(IComponent? component)
	{
	}

	[RequiresUnreferencedCode("The Type of components in the container cannot be statically discovered to validate the name.")]
	public virtual void Add(IComponent? component, string? name)
	{
	}

	protected virtual ISite CreateSite(IComponent component, string? name)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~Container()
	{
	}

	protected virtual object? GetService(Type service)
	{
		throw null;
	}

	public virtual void Remove(IComponent? component)
	{
	}

	protected void RemoveWithoutUnsiting(IComponent? component)
	{
	}

	[RequiresUnreferencedCode("The Type of components in the container cannot be statically discovered.")]
	protected virtual void ValidateName(IComponent component, string? name)
	{
	}
}
