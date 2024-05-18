using System.ComponentModel;
using System.Data.Common;

namespace System.Data.Odbc;

[Designer("Microsoft.VSDesigner.Data.VS.OdbcDataAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxItem("Microsoft.VSDesigner.Data.VS.OdbcDataAdapterToolboxItem, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public sealed class OdbcDataAdapter : DbDataAdapter, IDataAdapter, IDbDataAdapter, ICloneable
{
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OdbcCommand? DeleteCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OdbcCommand? InsertCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OdbcCommand? SelectCommand
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

	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public new OdbcCommand? UpdateCommand
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event OdbcRowUpdatedEventHandler? RowUpdated
	{
		add
		{
		}
		remove
		{
		}
	}

	public event OdbcRowUpdatingEventHandler? RowUpdating
	{
		add
		{
		}
		remove
		{
		}
	}

	public OdbcDataAdapter()
	{
	}

	public OdbcDataAdapter(OdbcCommand? selectCommand)
	{
	}

	public OdbcDataAdapter(string? selectCommandText, OdbcConnection? selectConnection)
	{
	}

	public OdbcDataAdapter(string? selectCommandText, string? selectConnectionString)
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
