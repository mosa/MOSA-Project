using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace System.Data.OleDb;

[DefaultEvent("InfoMessage")]
public sealed class OleDbConnection : DbConnection, IDbConnection, IDisposable, ICloneable
{
	[DefaultValue("")]
	[Editor("Microsoft.VSDesigner.Data.ADO.Design.OleDbConnectionStringEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[RecommendedAsConfigurable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[SettingsBindable(true)]
	public override string ConnectionString
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ConnectionTimeout
	{
		get
		{
			throw null;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Database
	{
		get
		{
			throw null;
		}
	}

	[Browsable(true)]
	public override string DataSource
	{
		get
		{
			throw null;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string Provider
	{
		get
		{
			throw null;
		}
	}

	public override string ServerVersion
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ConnectionState State
	{
		get
		{
			throw null;
		}
	}

	public event OleDbInfoMessageEventHandler? InfoMessage
	{
		add
		{
		}
		remove
		{
		}
	}

	public OleDbConnection()
	{
	}

	public OleDbConnection(string? connectionString)
	{
	}

	protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
	{
		throw null;
	}

	public new OleDbTransaction BeginTransaction()
	{
		throw null;
	}

	public new OleDbTransaction BeginTransaction(IsolationLevel isolationLevel)
	{
		throw null;
	}

	public override void ChangeDatabase(string value)
	{
	}

	public override void Close()
	{
	}

	public new OleDbCommand CreateCommand()
	{
		throw null;
	}

	protected override DbCommand CreateDbCommand()
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override void EnlistTransaction(Transaction? transaction)
	{
	}

	public DataTable? GetOleDbSchemaTable(Guid schema, object?[]? restrictions)
	{
		throw null;
	}

	public override DataTable GetSchema()
	{
		throw null;
	}

	public override DataTable GetSchema(string collectionName)
	{
		throw null;
	}

	public override DataTable GetSchema(string collectionName, string?[]? restrictionValues)
	{
		throw null;
	}

	public override void Open()
	{
	}

	public static void ReleaseObjectPool()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void ResetState()
	{
	}

	object ICloneable.Clone()
	{
		throw null;
	}
}
