using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[DefaultProperty("RelationName")]
[Editor("Microsoft.VSDesigner.Data.Design.DataRelationEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[TypeConverter(typeof(RelationshipConverter))]
public class DataRelation
{
	public virtual DataColumn[] ChildColumns
	{
		get
		{
			throw null;
		}
	}

	public virtual ForeignKeyConstraint? ChildKeyConstraint
	{
		get
		{
			throw null;
		}
	}

	public virtual DataTable ChildTable
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual DataSet? DataSet
	{
		get
		{
			throw null;
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

	[DefaultValue(false)]
	public virtual bool Nested
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual DataColumn[] ParentColumns
	{
		get
		{
			throw null;
		}
	}

	public virtual UniqueConstraint? ParentKeyConstraint
	{
		get
		{
			throw null;
		}
	}

	public virtual DataTable ParentTable
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue("")]
	public virtual string RelationName
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

	public DataRelation(string? relationName, DataColumn parentColumn, DataColumn childColumn)
	{
	}

	public DataRelation(string? relationName, DataColumn parentColumn, DataColumn childColumn, bool createConstraints)
	{
	}

	public DataRelation(string? relationName, DataColumn[] parentColumns, DataColumn[] childColumns)
	{
	}

	public DataRelation(string? relationName, DataColumn[] parentColumns, DataColumn[] childColumns, bool createConstraints)
	{
	}

	[Browsable(false)]
	public DataRelation(string relationName, string? parentTableName, string? parentTableNamespace, string? childTableName, string? childTableNamespace, string[]? parentColumnNames, string[]? childColumnNames, bool nested)
	{
	}

	[Browsable(false)]
	public DataRelation(string relationName, string? parentTableName, string? childTableName, string[]? parentColumnNames, string[]? childColumnNames, bool nested)
	{
	}

	protected void CheckStateForProperty()
	{
	}

	protected internal void OnPropertyChanging(PropertyChangedEventArgs pcevent)
	{
	}

	protected internal void RaisePropertyChanging(string name)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
