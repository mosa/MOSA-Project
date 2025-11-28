using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations.Schema;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class ColumnAttribute : Attribute
{
	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public int Order
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? TypeName
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

	public ColumnAttribute()
	{
	}

	public ColumnAttribute(string name)
	{
	}
}
