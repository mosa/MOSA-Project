using System.ComponentModel;
using System.Data.Common;

namespace System.Data.OleDb;

[Designer("Microsoft.VSDesigner.Data.VS.OleDbDataAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxItem("Microsoft.VSDesigner.Data.VS.OleDbDataAdapterToolboxItem, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public sealed class OleDbDataAdapter : DbDataAdapter, IDataAdapter, IDbDataAdapter, ICloneable
{
	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OleDbCommand? DeleteCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OleDbCommand? InsertCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OleDbCommand? SelectCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	IDbCommand? IDbDataAdapter.DeleteCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	IDbCommand? IDbDataAdapter.InsertCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	IDbCommand? IDbDataAdapter.SelectCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	IDbCommand? IDbDataAdapter.UpdateCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OleDbCommand? UpdateCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event OleDbRowUpdatedEventHandler? RowUpdated
	{
		add
		{
		}
		remove
		{
		}
	}

	public event OleDbRowUpdatingEventHandler? RowUpdating
	{
		add
		{
		}
		remove
		{
		}
	}

	public OleDbDataAdapter()
	{
	}

	public OleDbDataAdapter(OleDbCommand? selectCommand)
	{
	}

	public OleDbDataAdapter(string? selectCommandText, OleDbConnection? selectConnection)
	{
	}

	public OleDbDataAdapter(string? selectCommandText, string? selectConnectionString)
	{
	}

	protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand? command, StatementType statementType, DataTableMapping tableMapping)
	{
		throw null;
	}

	protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand? command, StatementType statementType, DataTableMapping tableMapping)
	{
		throw null;
	}

	public int Fill(DataSet dataSet, object ADODBRecordSet, string srcTable)
	{
		throw null;
	}

	public int Fill(DataTable dataTable, object ADODBRecordSet)
	{
		throw null;
	}

	protected override void OnRowUpdated(RowUpdatedEventArgs value)
	{
	}

	protected override void OnRowUpdating(RowUpdatingEventArgs value)
	{
	}

	object ICloneable.Clone()
	{
		throw null;
	}
}
