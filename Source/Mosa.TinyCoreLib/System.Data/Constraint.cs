using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[DefaultProperty("ConstraintName")]
[TypeConverter(typeof(ConstraintConverter))]
public abstract class Constraint
{
	[DefaultValue("")]
	public virtual string ConstraintName
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
	public PropertyCollection ExtendedProperties
	{
		get
		{
			throw null;
		}
	}

	public abstract DataTable? Table { get; }

	[CLSCompliant(false)]
	protected virtual DataSet? _DataSet
	{
		get
		{
			throw null;
		}
	}

	internal Constraint()
	{
	}

	protected void CheckStateForProperty()
	{
	}

	protected internal void SetDataSet(DataSet dataSet)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
