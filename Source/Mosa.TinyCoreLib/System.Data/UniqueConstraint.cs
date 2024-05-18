using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[DefaultProperty("ConstraintName")]
[Editor("Microsoft.VSDesigner.Data.Design.UniqueConstraintEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class UniqueConstraint : Constraint
{
	[ReadOnly(true)]
	public virtual DataColumn[] Columns
	{
		get
		{
			throw null;
		}
	}

	public bool IsPrimaryKey
	{
		get
		{
			throw null;
		}
	}

	[ReadOnly(true)]
	public override DataTable? Table
	{
		get
		{
			throw null;
		}
	}

	public UniqueConstraint(DataColumn column)
	{
	}

	public UniqueConstraint(DataColumn column, bool isPrimaryKey)
	{
	}

	public UniqueConstraint(DataColumn[] columns)
	{
	}

	public UniqueConstraint(DataColumn[] columns, bool isPrimaryKey)
	{
	}

	public UniqueConstraint(string? name, DataColumn column)
	{
	}

	public UniqueConstraint(string? name, DataColumn column, bool isPrimaryKey)
	{
	}

	public UniqueConstraint(string? name, DataColumn[] columns)
	{
	}

	public UniqueConstraint(string? name, DataColumn[] columns, bool isPrimaryKey)
	{
	}

	[Browsable(false)]
	public UniqueConstraint(string? name, string[]? columnNames, bool isPrimaryKey)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? key2)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
