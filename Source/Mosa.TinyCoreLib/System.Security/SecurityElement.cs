using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.Security;

public sealed class SecurityElement
{
	public Hashtable? Attributes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ArrayList? Children
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Tag
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Text
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityElement(string tag)
	{
	}

	public SecurityElement(string tag, string? text)
	{
	}

	public void AddAttribute(string name, string value)
	{
	}

	public void AddChild(SecurityElement child)
	{
	}

	public string? Attribute(string name)
	{
		throw null;
	}

	public SecurityElement Copy()
	{
		throw null;
	}

	public bool Equal([NotNullWhen(true)] SecurityElement? other)
	{
		throw null;
	}

	[return: NotNullIfNotNull("str")]
	public static string? Escape(string? str)
	{
		throw null;
	}

	public static SecurityElement? FromString(string xml)
	{
		throw null;
	}

	public static bool IsValidAttributeName([NotNullWhen(true)] string? name)
	{
		throw null;
	}

	public static bool IsValidAttributeValue([NotNullWhen(true)] string? value)
	{
		throw null;
	}

	public static bool IsValidTag([NotNullWhen(true)] string? tag)
	{
		throw null;
	}

	public static bool IsValidText([NotNullWhen(true)] string? text)
	{
		throw null;
	}

	public SecurityElement? SearchForChildByTag(string tag)
	{
		throw null;
	}

	public string? SearchForTextOfTag(string tag)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
