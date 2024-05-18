namespace System.ComponentModel.Design;

public class ServiceContainer : IServiceContainer, IServiceProvider, IDisposable
{
	protected virtual Type[] DefaultServices
	{
		get
		{
			throw null;
		}
	}

	public ServiceContainer()
	{
	}

	public ServiceContainer(IServiceProvider? parentProvider)
	{
	}

	public void AddService(Type serviceType, ServiceCreatorCallback callback)
	{
	}

	public virtual void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
	{
	}

	public void AddService(Type serviceType, object serviceInstance)
	{
	}

	public virtual void AddService(Type serviceType, object serviceInstance, bool promote)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual object? GetService(Type serviceType)
	{
		throw null;
	}

	public void RemoveService(Type serviceType)
	{
	}

	public virtual void RemoveService(Type serviceType, bool promote)
	{
	}
}
