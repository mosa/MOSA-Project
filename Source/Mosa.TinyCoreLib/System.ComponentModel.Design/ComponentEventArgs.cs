namespace System.ComponentModel.Design;

public class ComponentEventArgs : EventArgs
{
	public virtual IComponent? Component
	{
		get
		{
			throw null;
		}
	}

	public ComponentEventArgs(IComponent? component)
	{
	}
}
