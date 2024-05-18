using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class DisplayAttribute : Attribute
{
	public bool AutoGenerateField
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AutoGenerateFilter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? GroupName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
		set
		{
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

	public string? Prompt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
	public Type? ResourceType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? ShortName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool? GetAutoGenerateField()
	{
		throw null;
	}

	public bool? GetAutoGenerateFilter()
	{
		throw null;
	}

	public string? GetDescription()
	{
		throw null;
	}

	public string? GetGroupName()
	{
		throw null;
	}

	public string? GetName()
	{
		throw null;
	}

	public int? GetOrder()
	{
		throw null;
	}

	public string? GetPrompt()
	{
		throw null;
	}

	public string? GetShortName()
	{
		throw null;
	}
}
