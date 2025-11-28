namespace System.ComponentModel.Design;

public sealed class ComponentChangedEventArgs : EventArgs
{
	public object? Component
	{
		get
		{
			throw null;
		}
	}

	public MemberDescriptor? Member
	{
		get
		{
			throw null;
		}
	}

	public object? NewValue
	{
		get
		{
			throw null;
		}
	}

	public object? OldValue
	{
		get
		{
			throw null;
		}
	}

	public ComponentChangedEventArgs(object? component, MemberDescriptor? member, object? oldValue, object? newValue)
	{
	}
}
