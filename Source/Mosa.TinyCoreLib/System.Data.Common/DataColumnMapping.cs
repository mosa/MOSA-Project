using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

[TypeConverter(typeof(DataColumnMappingConverter))]
public sealed class DataColumnMapping : MarshalByRefObject, IColumnMapping, ICloneable
{
	internal sealed class DataColumnMappingConverter
	{
	}

	[DefaultValue("")]
	public string DataSetColumn
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
	public string SourceColumn
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

	public DataColumnMapping()
	{
	}

	public DataColumnMapping(string? sourceColumn, string? dataSetColumn)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public DataColumn? GetDataColumnBySchemaAction(DataTable dataTable, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type? dataType, MissingSchemaAction schemaAction)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static DataColumn? GetDataColumnBySchemaAction(string? sourceColumn, string? dataSetColumn, DataTable dataTable, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type? dataType, MissingSchemaAction schemaAction)
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
