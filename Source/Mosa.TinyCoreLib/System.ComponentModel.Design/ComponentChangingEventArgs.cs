namespace System.ComponentModel.Design;

public sealed class ComponentChangingEventArgs : EventArgs
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

	public ComponentChangingEventArgs(object? component, MemberDescriptor? member)
	{
	}
}
