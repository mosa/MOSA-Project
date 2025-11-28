using System.Collections;
using System.ComponentModel;

namespace System.Data.Common;

[Editor("Microsoft.VSDesigner.Data.Design.DataTableMappingCollectionEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ListBindable(false)]
public sealed class DataTableMappingCollection : MarshalByRefObject, ICollection, IEnumerable, IList, ITableMappingCollection
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Count
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DataTableMapping this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DataTableMapping this[string sourceTable]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	bool IList.IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	bool IList.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IList.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	object ITableMappingCollection.this[string index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Add(object? value)
	{
		throw null;
	}

	public DataTableMapping Add(string? sourceTable, string? dataSetTable)
	{
		throw null;
	}

	public void AddRange(Array values)
	{
	}

	public void AddRange(DataTableMapping[] values)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(object? value)
	{
		throw null;
	}

	public bool Contains(string? value)
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(DataTableMapping[] array, int index)
	{
	}

	public DataTableMapping GetByDataSetTable(string dataSetTable)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static DataTableMapping? GetTableMappingBySchemaAction(DataTableMappingCollection? tableMappings, string sourceTable, string dataSetTable, MissingMappingAction mappingAction)
	{
		throw null;
	}

	public int IndexOf(object? value)
	{
		throw null;
	}

	public int IndexOf(string? sourceTable)
	{
		throw null;
	}

	public int IndexOfDataSetTable(string? dataSetTable)
	{
		throw null;
	}

	public void Insert(int index, DataTableMapping value)
	{
	}

	public void Insert(int index, object? value)
	{
	}

	public void Remove(DataTableMapping value)
	{
	}

	public void Remove(object? value)
	{
	}

	public void RemoveAt(int index)
	{
	}

	public void RemoveAt(string sourceTable)
	{
	}

	ITableMapping ITableMappingCollection.Add(string sourceTableName, string dataSetTableName)
	{
		throw null;
	}

	ITableMapping ITableMappingCollection.GetByDataSetTable(string dataSetTableName)
	{
		throw null;
	}
}
