using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

public sealed class DataColumnMappingCollection : MarshalByRefObject, ICollection, IEnumerable, IList, IColumnMappingCollection
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
	public DataColumnMapping this[int index]
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
	public DataColumnMapping this[string sourceColumn]
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

	object IColumnMappingCollection.this[string index]
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

	public DataColumnMapping Add(string? sourceColumn, string? dataSetColumn)
	{
		throw null;
	}

	public void AddRange(Array values)
	{
	}

	public void AddRange(DataColumnMapping[] values)
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

	public void CopyTo(DataColumnMapping[] array, int index)
	{
	}

	public DataColumnMapping GetByDataSetColumn(string value)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static DataColumnMapping? GetColumnMappingBySchemaAction(DataColumnMappingCollection? columnMappings, string sourceColumn, MissingMappingAction mappingAction)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static DataColumn? GetDataColumn(DataColumnMappingCollection? columnMappings, string sourceColumn, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type? dataType, DataTable dataTable, MissingMappingAction mappingAction, MissingSchemaAction schemaAction)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(object? value)
	{
		throw null;
	}

	public int IndexOf(string? sourceColumn)
	{
		throw null;
	}

	public int IndexOfDataSetColumn(string? dataSetColumn)
	{
		throw null;
	}

	public void Insert(int index, DataColumnMapping value)
	{
	}

	public void Insert(int index, object? value)
	{
	}

	public void Remove(DataColumnMapping value)
	{
	}

	public void Remove(object? value)
	{
	}

	public void RemoveAt(int index)
	{
	}

	public void RemoveAt(string sourceColumn)
	{
	}

	IColumnMapping IColumnMappingCollection.Add(string? sourceColumnName, string? dataSetColumnName)
	{
		throw null;
	}

	IColumnMapping IColumnMappingCollection.GetByDataSetColumn(string dataSetColumnName)
	{
		throw null;
	}
}
