using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

[TypeConverter(typeof(DataTableMappingConverter))]
public sealed class DataTableMapping : MarshalByRefObject, ITableMapping, ICloneable
{
	internal sealed class DataTableMappingConverter
	{
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataColumnMappingCollection ColumnMappings
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue("")]
	public string DataSetTable
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	[DefaultValue("")]
	public string SourceTable
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	IColumnMappingCollection ITableMapping.ColumnMappings
	{
		get
		{
			throw null;
		}
	}

	public DataTableMapping()
	{
	}

	public DataTableMapping(string? sourceTable, string? dataSetTable)
	{
	}

	public DataTableMapping(string? sourceTable, string? dataSetTable, DataColumnMapping[]? columnMappings)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public DataColumnMapping? GetColumnMappingBySchemaAction(string sourceColumn, MissingMappingAction mappingAction)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public DataColumn? GetDataColumn(string sourceColumn, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type? dataType, DataTable dataTable, MissingMappingAction mappingAction, MissingSchemaAction schemaAction)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public DataTable? GetDataTableBySchemaAction(DataSet dataSet, MissingSchemaAction schemaAction)
	{
		throw null;
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
