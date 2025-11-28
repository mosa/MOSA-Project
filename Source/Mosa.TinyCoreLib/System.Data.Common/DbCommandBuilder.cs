using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

public abstract class DbCommandBuilder : Component
{
	[DefaultValue(CatalogLocation.Start)]
	public virtual CatalogLocation CatalogLocation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(".")]
	public virtual string CatalogSeparator
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

	[DefaultValue(ConflictOption.CompareAllSearchableValues)]
	public virtual ConflictOption ConflictOption
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
	public DbDataAdapter? DataAdapter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue("")]
	public virtual string QuotePrefix
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
	public virtual string QuoteSuffix
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

	[DefaultValue(".")]
	public virtual string SchemaSeparator
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

	[DefaultValue(false)]
	public bool SetAllValues
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected abstract void ApplyParameterInfo(DbParameter parameter, DataRow row, StatementType statementType, bool whereClause);

	protected override void Dispose(bool disposing)
	{
	}

	public DbCommand GetDeleteCommand()
	{
		throw null;
	}

	public DbCommand GetDeleteCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	public DbCommand GetInsertCommand()
	{
		throw null;
	}

	public DbCommand GetInsertCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	protected abstract string GetParameterName(int parameterOrdinal);

	protected abstract string GetParameterName(string parameterName);

	protected abstract string GetParameterPlaceholder(int parameterOrdinal);

	protected virtual DataTable? GetSchemaTable(DbCommand sourceCommand)
	{
		throw null;
	}

	public DbCommand GetUpdateCommand()
	{
		throw null;
	}

	public DbCommand GetUpdateCommand(bool useColumnsForParameterNames)
	{
		throw null;
	}

	protected virtual DbCommand InitializeCommand(DbCommand? command)
	{
		throw null;
	}

	public virtual string QuoteIdentifier(string unquotedIdentifier)
	{
		throw null;
	}

	public virtual void RefreshSchema()
	{
	}

	protected void RowUpdatingHandler(RowUpdatingEventArgs rowUpdatingEvent)
	{
	}

	protected abstract void SetRowUpdatingHandler(DbDataAdapter adapter);

	public virtual string UnquoteIdentifier(string quotedIdentifier)
	{
		throw null;
	}
}
