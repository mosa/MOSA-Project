using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public interface IDataAdapter
{
	MissingMappingAction MissingMappingAction { get; set; }

	MissingSchemaAction MissingSchemaAction { get; set; }

	ITableMappingCollection TableMappings { get; }

	int Fill(DataSet dataSet);

	[RequiresUnreferencedCode("IDataReader's (built from adapter commands) schema table types cannot be statically analyzed.")]
	DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType);

	IDataParameter[] GetFillParameters();

	[RequiresUnreferencedCode("IDataReader's (built from adapter commands) schema table types cannot be statically analyzed.")]
	int Update(DataSet dataSet);
}
