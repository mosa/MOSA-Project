namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
[Conditional("CODE_ANALYSIS")]
public sealed class SuppressMessageAttribute : Attribute
{
	public string Category
	{
		get
		{
			throw null;
		}
	}

	public string CheckId
	{
		get
		{
			throw null;
		}
	}

	public string? Justification
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? MessageId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Scope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SuppressMessageAttribute(string category, string checkId)
	{
	}
}
