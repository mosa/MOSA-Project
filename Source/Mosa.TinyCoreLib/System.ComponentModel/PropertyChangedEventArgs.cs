namespace System.ComponentModel;

public class PropertyChangedEventArgs : EventArgs
{
	public virtual string? PropertyName
	{
		get
		{
			throw null;
		}
	}

	public PropertyChangedEventArgs(string? propertyName)
	{
	}
}
