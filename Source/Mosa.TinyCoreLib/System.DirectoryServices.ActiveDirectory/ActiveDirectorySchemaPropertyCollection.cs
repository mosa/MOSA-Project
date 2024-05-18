using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySchemaPropertyCollection : CollectionBase
{
	public ActiveDirectorySchemaProperty this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ActiveDirectorySchemaPropertyCollection()
	{
	}

	public int Add(ActiveDirectorySchemaProperty schemaProperty)
	{
		throw null;
	}

	public void AddRange(ActiveDirectorySchemaPropertyCollection properties)
	{
	}

	public void AddRange(ActiveDirectorySchemaProperty[] properties)
	{
	}

	public void AddRange(ReadOnlyActiveDirectorySchemaPropertyCollection properties)
	{
	}

	public bool Contains(ActiveDirectorySchemaProperty schemaProperty)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySchemaProperty[] properties, int index)
	{
	}

	public int IndexOf(ActiveDirectorySchemaProperty schemaProperty)
	{
		throw null;
	}

	public void Insert(int index, ActiveDirectorySchemaProperty schemaProperty)
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

	public void Remove(ActiveDirectorySchemaProperty schemaProperty)
	{
	}
}
