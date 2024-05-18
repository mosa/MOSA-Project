using System.Collections;

namespace System.DirectoryServices;

public class PropertyValueCollection : CollectionBase
{
	public object? this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string PropertyName
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal PropertyValueCollection()
	{
	}

	public int Add(object? value)
	{
		throw null;
	}

	public void AddRange(PropertyValueCollection value)
	{
	}

	public void AddRange(object?[] value)
	{
	}

	public bool Contains(object? value)
	{
		throw null;
	}

	public void CopyTo(object?[] array, int index)
	{
	}

	public int IndexOf(object? value)
	{
		throw null;
	}

	public void Insert(int index, object? value)
	{
	}

	protected override void OnClearComplete()
	{
	}

	protected override void OnInsertComplete(int index, object? value)
	{
	}

	protected override void OnRemoveComplete(int index, object? value)
	{
	}

	protected override void OnSetComplete(int index, object? oldValue, object? newValue)
	{
	}

	public void Remove(object? value)
	{
	}
}
