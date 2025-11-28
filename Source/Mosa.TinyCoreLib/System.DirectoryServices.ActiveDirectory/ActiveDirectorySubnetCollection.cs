using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySubnetCollection : CollectionBase
{
	public ActiveDirectorySubnet this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ActiveDirectorySubnetCollection()
	{
	}

	public int Add(ActiveDirectorySubnet subnet)
	{
		throw null;
	}

	public void AddRange(ActiveDirectorySubnetCollection subnets)
	{
	}

	public void AddRange(ActiveDirectorySubnet[] subnets)
	{
	}

	public bool Contains(ActiveDirectorySubnet subnet)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySubnet[] array, int index)
	{
	}

	public int IndexOf(ActiveDirectorySubnet subnet)
	{
		throw null;
	}

	public void Insert(int index, ActiveDirectorySubnet subnet)
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

	public void Remove(ActiveDirectorySubnet subnet)
	{
	}
}
