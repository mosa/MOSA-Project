using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations.Schema;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TableAttribute : Attribute
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string? Schema
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

	public TableAttribute(string name)
	{
	}
}
