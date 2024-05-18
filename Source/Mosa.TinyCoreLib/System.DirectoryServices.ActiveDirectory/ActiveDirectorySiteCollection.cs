using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySiteCollection : CollectionBase
{
	public ActiveDirectorySite this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ActiveDirectorySiteCollection()
	{
	}

	public int Add(ActiveDirectorySite site)
	{
		throw null;
	}

	public void AddRange(ActiveDirectorySiteCollection sites)
	{
	}

	public void AddRange(ActiveDirectorySite[] sites)
	{
	}

	public bool Contains(ActiveDirectorySite site)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySite[] array, int index)
	{
	}

	public int IndexOf(ActiveDirectorySite site)
	{
		throw null;
	}

	public void Insert(int index, ActiveDirectorySite site)
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

	protected override void OnValidate(object value)
	{
	}

	public void Remove(ActiveDirectorySite site)
	{
	}
}
