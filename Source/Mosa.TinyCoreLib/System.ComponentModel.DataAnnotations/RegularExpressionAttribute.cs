using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class RegularExpressionAttribute : ValidationAttribute
{
	public TimeSpan MatchTimeout
	{
		get
		{
			throw null;
		}
	}

	public int MatchTimeoutInMilliseconds
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Pattern
	{
		get
		{
			throw null;
		}
	}

	public RegularExpressionAttribute([StringSyntax("Regex")] string pattern)
	{
	}

	public override string FormatErrorMessage(string name)
	{
		throw null;
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
