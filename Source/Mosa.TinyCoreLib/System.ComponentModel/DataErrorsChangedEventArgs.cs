namespace System.ComponentModel;

public class DataErrorsChangedEventArgs : EventArgs
{
	public virtual string? PropertyName
	{
		get
		{
			throw null;
		}
	}

	public DataErrorsChangedEventArgs(string? propertyName)
	{
	}
}
