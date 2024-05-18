using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class DirectoryServerCollection : CollectionBase
{
	public DirectoryServer this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal DirectoryServerCollection()
	{
	}

	public int Add(DirectoryServer server)
	{
		throw null;
	}

	public void AddRange(DirectoryServer[] servers)
	{
	}

	public bool Contains(DirectoryServer server)
	{
		throw null;
	}

	public void CopyTo(DirectoryServer[] array, int index)
	{
	}

	public int IndexOf(DirectoryServer server)
	{
		throw null;
	}

	public void Insert(int index, DirectoryServer server)
	{
	}

	protected override void OnClear()
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

	public void Remove(DirectoryServer server)
	{
	}
}
