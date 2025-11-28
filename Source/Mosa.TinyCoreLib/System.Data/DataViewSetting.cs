using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class DataViewSetting
{
	public bool ApplyDefaultSort
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
	public DataViewManager? DataViewManager
	{
		get
		{
			throw null;
		}
	}

	public string RowFilter
	{
		get
		{
			throw null;
		}
		[RequiresUnreferencedCode("Members of types used in the filter expression might be trimmed.")]
		[param: AllowNull]
		set
		{
		}
	}

	public DataViewRowState RowStateFilter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Sort
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
	public DataTable? Table
	{
		get
		{
			throw null;
		}
	}

	internal DataViewSetting()
	{
	}
}
