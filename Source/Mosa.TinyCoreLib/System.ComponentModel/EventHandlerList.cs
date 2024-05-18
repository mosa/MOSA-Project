namespace System.ComponentModel;

public sealed class EventHandlerList : IDisposable
{
	public Delegate? this[object key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public void AddHandler(object key, Delegate? value)
	{
	}

	public void AddHandlers(EventHandlerList listToAddFrom)
	{
	}

	public void Dispose()
	{
	}

	public void RemoveHandler(object key, Delegate? value)
	{
	}
}
