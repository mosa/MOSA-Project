using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[DefaultProperty("ColumnName")]
[DesignTimeVisible(false)]
[Editor("Microsoft.VSDesigner.Data.Design.DataColumnEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[ToolboxItem(false)]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
public class DataColumn : MarshalByValueComponent
{
	[DefaultValue(true)]
	public bool AllowDBNull
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
	[RefreshProperties(RefreshProperties.All)]
	public bool AutoIncrement
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(0L)]
	public long AutoIncrementSeed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(1L)]
	public long AutoIncrementStep
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Caption
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

	[DefaultValue(MappingType.Element)]
	public virtual MappingType ColumnMapping
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
	[RefreshProperties(RefreshProperties.All)]
	public string ColumnName
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

	[DefaultValue(DataSetDateTime.UnspecifiedLocal)]
	[RefreshProperties(RefreshProperties.All)]
	public DataSetDateTime DateTimeMode
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
	[RefreshProperties(RefreshProperties.All)]
	public string Expression
	{
		get
		{
			throw null;
		}
		[RequiresUnreferencedCode("Members from types used in the expressions may be trimmed if not referenced directly.")]
		[param: AllowNull]
		set
		{
		}
	}

	[Browsable(false)]
	public PropertyCollection ExtendedProperties
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(-1)]
	public int MaxLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Namespace
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Ordinal
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue("")]
	public string Prefix
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
	public bool ReadOnly
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
	public DataTable? Table
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Unique
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(typeof(string))]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(ColumnTypeConverter))]
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	public Type DataType
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

	[TypeConverter(typeof(DefaultValueTypeConverter))]
	public object DefaultValue
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

	public DataColumn()
	{
	}

	public DataColumn(string? columnName)
	{
	}

	public DataColumn(string? columnName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type dataType)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types or types used in expressions may be trimmed if not referenced directly.")]
	public DataColumn(string? columnName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type dataType, string? expr)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types or types used in expressions may be trimmed if not referenced directly.")]
	public DataColumn(string? columnName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type dataType, string? expr, MappingType type)
	{
	}

	protected internal void CheckNotAllowNull()
	{
	}

	protected void CheckUnique()
	{
	}

	protected virtual void OnPropertyChanging(PropertyChangedEventArgs pcevent)
	{
	}

	protected internal void RaisePropertyChanging(string name)
	{
	}

	public void SetOrdinal(int ordinal)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
