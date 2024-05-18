using System.Collections;
using System.ComponentModel;

namespace System.Data;

[DefaultEvent("CollectionChanged")]
[Editor("Microsoft.VSDesigner.Data.Design.TablesCollectionEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ListBindable(false)]
public sealed class DataTableCollection : InternalDataCollectionBase
{
	public DataTable this[int index]
	{
		get
		{
			throw null;
		}
	}

	public DataTable? this[string? name]
	{
		get
		{
			throw null;
		}
	}

	public DataTable? this[string? name, string tableNamespace]
	{
		get
		{
			throw null;
		}
	}

	protected override ArrayList List
	{
		get
		{
			throw null;
		}
	}

	public event CollectionChangeEventHandler? CollectionChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event CollectionChangeEventHandler? CollectionChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	internal DataTableCollection()
	{
	}

	public DataTable Add()
	{
		throw null;
	}

	public void Add(DataTable table)
	{
	}

	public DataTable Add(string? name)
	{
		throw null;
	}

	public DataTable Add(string? name, string? tableNamespace)
	{
		throw null;
	}

	public void AddRange(DataTable?[]? tables)
	{
	}

	public bool CanRemove(DataTable? table)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(string? name)
	{
		throw null;
	}

	public bool Contains(string name, string tableNamespace)
	{
		throw null;
	}

	public void CopyTo(DataTable[] array, int index)
	{
	}

	public int IndexOf(DataTable? table)
	{
		throw null;
	}

	public int IndexOf(string? tableName)
	{
		throw null;
	}

	public int IndexOf(string tableName, string tableNamespace)
	{
		throw null;
	}

	public void Remove(DataTable table)
	{
	}

	public void Remove(string name)
	{
	}

	public void Remove(string name, string tableNamespace)
	{
	}

	public void RemoveAt(int index)
	{
	}
}
