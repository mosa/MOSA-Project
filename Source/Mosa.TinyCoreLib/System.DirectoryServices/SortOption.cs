using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.DirectoryServices;

[TypeConverter(typeof(ExpandableObjectConverter))]
public class SortOption
{
	[DefaultValue(SortDirection.Ascending)]
	public SortDirection Direction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	public string? PropertyName
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public SortOption()
	{
	}

	public SortOption(string propertyName, SortDirection direction)
	{
	}
}
