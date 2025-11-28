using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Odbc;

public sealed class OdbcParameter : DbParameter, IDataParameter, IDbDataParameter, ICloneable
{
	public override DbType DbType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override ParameterDirection Direction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool IsNullable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(OdbcType.NChar)]
	[DbProviderSpecificTypeProperty(true)]
	public OdbcType OdbcType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string ParameterName
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

	public new byte Precision
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public new byte Scale
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int Size
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string SourceColumn
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

	public override bool SourceColumnNullMapping
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override DataRowVersion SourceVersion
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override object? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public OdbcParameter()
	{
	}

	public OdbcParameter(string? name, OdbcType type)
	{
	}

	public OdbcParameter(string? name, OdbcType type, int size)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public OdbcParameter(string? parameterName, OdbcType odbcType, int size, ParameterDirection parameterDirection, bool isNullable, byte precision, byte scale, string? srcColumn, DataRowVersion srcVersion, object? value)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public OdbcParameter(string? parameterName, OdbcType odbcType, int size, ParameterDirection parameterDirection, byte precision, byte scale, string? sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object? value)
	{
	}

	public OdbcParameter(string? name, OdbcType type, int size, string? sourcecolumn)
	{
	}

	public OdbcParameter(string? name, object? value)
	{
	}

	public override void ResetDbType()
	{
	}

	public void ResetOdbcType()
	{
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
