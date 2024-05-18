namespace System.ComponentModel;

public abstract class EventDescriptor : MemberDescriptor
{
	public abstract Type ComponentType { get; }

	public abstract Type EventType { get; }

	public abstract bool IsMulticast { get; }

	protected EventDescriptor(MemberDescriptor descr)
		: base((string)null)
	{
	}

	protected EventDescriptor(MemberDescriptor descr, Attribute[]? attrs)
		: base((string)null)
	{
	}

	protected EventDescriptor(string name, Attribute[]? attrs)
		: base((string)null)
	{
	}

	public abstract void AddEventHandler(object component, Delegate value);

	public abstract void RemoveEventHandler(object component, Delegate value);
}
