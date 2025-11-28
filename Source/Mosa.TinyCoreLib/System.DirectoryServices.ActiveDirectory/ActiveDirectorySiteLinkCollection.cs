using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySiteLinkCollection : CollectionBase
{
	public ActiveDirectorySiteLink this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ActiveDirectorySiteLinkCollection()
	{
	}

	public int Add(ActiveDirectorySiteLink link)
	{
		throw null;
	}

	public void AddRange(ActiveDirectorySiteLinkCollection links)
	{
	}

	public void AddRange(ActiveDirectorySiteLink[] links)
	{
	}

	public bool Contains(ActiveDirectorySiteLink link)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySiteLink[] array, int index)
	{
	}

	public int IndexOf(ActiveDirectorySiteLink link)
	{
		throw null;
	}

	public void Insert(int index, ActiveDirectorySiteLink link)
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

	public void Remove(ActiveDirectorySiteLink link)
	{
	}
}
