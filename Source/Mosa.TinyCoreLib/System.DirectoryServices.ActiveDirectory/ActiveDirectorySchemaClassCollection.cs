using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySchemaClassCollection : CollectionBase
{
	public ActiveDirectorySchemaClass this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ActiveDirectorySchemaClassCollection()
	{
	}

	public int Add(ActiveDirectorySchemaClass schemaClass)
	{
		throw null;
	}

	public void AddRange(ActiveDirectorySchemaClassCollection schemaClasses)
	{
	}

	public void AddRange(ActiveDirectorySchemaClass[] schemaClasses)
	{
	}

	public void AddRange(ReadOnlyActiveDirectorySchemaClassCollection schemaClasses)
	{
	}

	public bool Contains(ActiveDirectorySchemaClass schemaClass)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySchemaClass[] schemaClasses, int index)
	{
	}

	public int IndexOf(ActiveDirectorySchemaClass schemaClass)
	{
		throw null;
	}

	public void Insert(int index, ActiveDirectorySchemaClass schemaClass)
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

	public void Remove(ActiveDirectorySchemaClass schemaClass)
	{
	}
}
