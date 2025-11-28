using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
public class DataAdapter : Component, IDataAdapter
{
	[DefaultValue(true)]
	public bool AcceptChangesDuringFill
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	public bool AcceptChangesDuringUpdate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public bool ContinueUpdateOnError
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	public LoadOption FillLoadOption
	{
		get
		{
			throw null;
		}
		[RequiresUnreferencedCode("Using LoadOption may cause members from types used in the expression column to be trimmed if not referenced directly.")]
		set
		{
		}
	}

	[DefaultValue(MissingMappingAction.Passthrough)]
	public MissingMappingAction MissingMappingAction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(MissingSchemaAction.Add)]
	public MissingSchemaAction MissingSchemaAction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	public virtual bool ReturnProviderSpecificTypes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	ITableMappingCollection IDataAdapter.TableMappings
	{
		get
		{
			throw null;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataTableMappingCollection TableMappings
	{
		get
		{
			throw null;
		}
	}

	public event FillErrorEventHandler? FillError
	{
		add
		{
		}
		remove
		{
		}
	}

	protected DataAdapter()
	{
	}

	protected DataAdapter(DataAdapter from)
	{
	}

	[Obsolete("CloneInternals() has been deprecated. Use the DataAdapter(DataAdapter from) constructor instead.")]
	protected virtual DataAdapter CloneInternals()
	{
		throw null;
	}

	protected virtual DataTableMappingCollection CreateTableMappings()
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public virtual int Fill(DataSet dataSet)
	{
		throw null;
	}

	protected virtual int Fill(DataSet dataSet, string srcTable, IDataReader dataReader, int startRecord, int maxRecords)
	{
		throw null;
	}

	protected virtual int Fill(DataTable dataTable, IDataReader dataReader)
	{
		throw null;
	}

	protected virtual int Fill(DataTable[] dataTables, IDataReader dataReader, int startRecord, int maxRecords)
	{
		throw null;
	}

	[RequiresUnreferencedCode("IDataReader's (built from adapter commands) schema table types cannot be statically analyzed.")]
	public virtual DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("dataReader's schema table types cannot be statically analyzed.")]
	protected virtual DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, string srcTable, IDataReader dataReader)
	{
		throw null;
	}

	[RequiresUnreferencedCode("dataReader's schema table types cannot be statically analyzed.")]
	protected virtual DataTable? FillSchema(DataTable dataTable, SchemaType schemaType, IDataReader dataReader)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public virtual IDataParameter[] GetFillParameters()
	{
		throw null;
	}

	protected bool HasTableMappings()
	{
		throw null;
	}

	protected virtual void OnFillError(FillErrorEventArgs value)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFillLoadOption()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool ShouldSerializeAcceptChangesDuringFill()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool ShouldSerializeFillLoadOption()
	{
		throw null;
	}

	protected virtual bool ShouldSerializeTableMappings()
	{
		throw null;
	}

	[RequiresUnreferencedCode("IDataReader's (built from adapter commands) schema table types cannot be statically analyzed.")]
	public virtual int Update(DataSet dataSet)
	{
		throw null;
	}
}
