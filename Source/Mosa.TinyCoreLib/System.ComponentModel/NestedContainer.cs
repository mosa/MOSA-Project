namespace System.ComponentModel;

public class NestedContainer : Container, IContainer, IDisposable, INestedContainer
{
	public IComponent Owner
	{
		get
		{
			throw null;
		}
	}

	protected virtual string? OwnerName
	{
		get
		{
			throw null;
		}
	}

	public NestedContainer(IComponent owner)
	{
	}

	protected override ISite CreateSite(IComponent component, string? name)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected override object? GetService(Type service)
	{
		throw null;
	}
}
