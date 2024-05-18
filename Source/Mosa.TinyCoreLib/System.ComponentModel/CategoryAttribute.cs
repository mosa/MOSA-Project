using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public class CategoryAttribute : Attribute
{
	public static CategoryAttribute Action
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Appearance
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Asynchronous
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Behavior
	{
		get
		{
			throw null;
		}
	}

	public string Category
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Data
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Default
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Design
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute DragDrop
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Focus
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Format
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Key
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Layout
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute Mouse
	{
		get
		{
			throw null;
		}
	}

	public static CategoryAttribute WindowStyle
	{
		get
		{
			throw null;
		}
	}

	public CategoryAttribute()
	{
	}

	public CategoryAttribute(string category)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected virtual string? GetLocalizedString(string value)
	{
		throw null;
	}

	public override bool IsDefaultAttribute()
	{
		throw null;
	}
}
