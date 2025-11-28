using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[DefaultProperty("ConstraintName")]
[Editor("Microsoft.VSDesigner.Data.Design.ForeignKeyConstraintEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public class ForeignKeyConstraint : Constraint
{
	[DefaultValue(AcceptRejectRule.None)]
	public virtual AcceptRejectRule AcceptRejectRule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[ReadOnly(true)]
	public virtual DataColumn[] Columns
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(Rule.Cascade)]
	public virtual Rule DeleteRule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[ReadOnly(true)]
	public virtual DataColumn[] RelatedColumns
	{
		get
		{
			throw null;
		}
	}

	[ReadOnly(true)]
	public virtual DataTable RelatedTable
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

	[DefaultValue(Rule.Cascade)]
	public virtual Rule UpdateRule
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ForeignKeyConstraint(DataColumn parentColumn, DataColumn childColumn)
	{
	}

	public ForeignKeyConstraint(DataColumn[] parentColumns, DataColumn[] childColumns)
	{
	}

	public ForeignKeyConstraint(string? constraintName, DataColumn parentColumn, DataColumn childColumn)
	{
	}

	public ForeignKeyConstraint(string? constraintName, DataColumn[] parentColumns, DataColumn[] childColumns)
	{
	}

	[Browsable(false)]
	public ForeignKeyConstraint(string? constraintName, string? parentTableName, string? parentTableNamespace, string[] parentColumnNames, string[] childColumnNames, AcceptRejectRule acceptRejectRule, Rule deleteRule, Rule updateRule)
	{
	}

	[Browsable(false)]
	public ForeignKeyConstraint(string? constraintName, string? parentTableName, string[] parentColumnNames, string[] childColumnNames, AcceptRejectRule acceptRejectRule, Rule deleteRule, Rule updateRule)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? key)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
