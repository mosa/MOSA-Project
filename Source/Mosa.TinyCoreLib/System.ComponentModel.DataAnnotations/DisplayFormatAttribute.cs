using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DisplayFormatAttribute : Attribute
{
	public bool ApplyFormatInEditMode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ConvertEmptyStringToNull
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? DataFormatString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool HtmlEncode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? NullDisplayText
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
	public Type? NullDisplayTextResourceType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? GetNullDisplayText()
	{
		throw null;
	}
}
