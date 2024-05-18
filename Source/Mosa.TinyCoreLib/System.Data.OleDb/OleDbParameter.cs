using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.OleDb;

[TypeConverter(typeof(OleDbParameterConverter))]
public sealed class OleDbParameter : DbParameter, IDataParameter, IDbDataParameter, ICloneable
{
	internal sealed class OleDbParameterConverter : ExpandableObjectConverter
	{
	}

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

	[RefreshProperties(RefreshProperties.All)]
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

	[RefreshProperties(RefreshProperties.All)]
	[DbProviderSpecificTypeProperty(true)]
	public OleDbType OleDbType
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

	[DefaultValue(0)]
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

	[DefaultValue(0)]
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

	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(StringConverter))]
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

	public OleDbParameter()
	{
	}

	public OleDbParameter(string? name, OleDbType dataType)
	{
	}

	public OleDbParameter(string? name, OleDbType dataType, int size)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public OleDbParameter(string? parameterName, OleDbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string? srcColumn, DataRowVersion srcVersion, object? value)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public OleDbParameter(string? parameterName, OleDbType dbType, int size, ParameterDirection direction, byte precision, byte scale, string? sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object? value)
	{
	}

	public OleDbParameter(string? name, OleDbType dataType, int size, string? srcColumn)
	{
	}

	public OleDbParameter(string? name, object? value)
	{
	}

	public override void ResetDbType()
	{
	}

	public void ResetOleDbType()
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
