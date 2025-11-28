namespace System.ComponentModel;

public class ListChangedEventArgs : EventArgs
{
	public ListChangedType ListChangedType
	{
		get
		{
			throw null;
		}
	}

	public int NewIndex
	{
		get
		{
			throw null;
		}
	}

	public int OldIndex
	{
		get
		{
			throw null;
		}
	}

	public PropertyDescriptor? PropertyDescriptor
	{
		get
		{
			throw null;
		}
	}

	public ListChangedEventArgs(ListChangedType listChangedType, PropertyDescriptor? propDesc)
	{
	}

	public ListChangedEventArgs(ListChangedType listChangedType, int newIndex)
	{
	}

	public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, PropertyDescriptor? propDesc)
	{
	}

	public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex)
	{
	}
}
