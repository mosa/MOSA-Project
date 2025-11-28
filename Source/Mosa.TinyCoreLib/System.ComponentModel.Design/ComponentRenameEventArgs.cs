namespace System.ComponentModel.Design;

public class ComponentRenameEventArgs : EventArgs
{
	public object? Component
	{
		get
		{
			throw null;
		}
	}

	public virtual string? NewName
	{
		get
		{
			throw null;
		}
	}

	public virtual string? OldName
	{
		get
		{
			throw null;
		}
	}

	public ComponentRenameEventArgs(object? component, string? oldName, string? newName)
	{
	}
}
