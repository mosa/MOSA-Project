namespace System.ComponentModel;

public class PropertyChangingEventArgs : EventArgs
{
	public virtual string? PropertyName
	{
		get
		{
			throw null;
		}
	}

	public PropertyChangingEventArgs(string? propertyName)
	{
	}
}
