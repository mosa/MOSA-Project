namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class DisplayColumnAttribute : Attribute
{
	public string DisplayColumn
	{
		get
		{
			throw null;
		}
	}

	public string? SortColumn
	{
		get
		{
			throw null;
		}
	}

	public bool SortDescending
	{
		get
		{
			throw null;
		}
	}

	public DisplayColumnAttribute(string displayColumn)
	{
	}

	public DisplayColumnAttribute(string displayColumn, string? sortColumn)
	{
	}

	public DisplayColumnAttribute(string displayColumn, string? sortColumn, bool sortDescending)
	{
	}
}
