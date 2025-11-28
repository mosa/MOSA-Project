using System.Diagnostics.CodeAnalysis;

namespace System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class GeneratedRegexAttribute : Attribute
{
	public string CultureName { get; }

	public string Pattern { get; }

	public RegexOptions Options { get; }

	public int MatchTimeoutMilliseconds { get; }

	public GeneratedRegexAttribute([StringSyntax("Regex")] string pattern)
	{
	}

	public GeneratedRegexAttribute([StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
	}

	public GeneratedRegexAttribute([StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, string cultureName)
	{
	}

	public GeneratedRegexAttribute([StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, int matchTimeoutMilliseconds)
	{
	}

	public GeneratedRegexAttribute([StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, int matchTimeoutMilliseconds, string cultureName)
	{
	}
}
