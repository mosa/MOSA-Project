using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace System.Text.RegularExpressions;

public class Regex : ISerializable
{
	public ref struct ValueMatchEnumerator
	{
		private object _dummy;

		private int _dummyPrimitive;

		public readonly ValueMatch Current
		{
			get
			{
				throw null;
			}
		}

		public readonly ValueMatchEnumerator GetEnumerator()
		{
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}
	}

	protected internal Hashtable? capnames;

	protected internal Hashtable? caps;

	protected internal int capsize;

	protected internal string[]? capslist;

	protected internal RegexRunnerFactory? factory;

	public static readonly TimeSpan InfiniteMatchTimeout;

	protected internal TimeSpan internalMatchTimeout;

	[StringSyntax("Regex")]
	protected internal string? pattern;

	protected internal RegexOptions roptions;

	public static int CacheSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[CLSCompliant(false)]
	protected IDictionary? CapNames
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

	[CLSCompliant(false)]
	protected IDictionary? Caps
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

	public TimeSpan MatchTimeout
	{
		get
		{
			throw null;
		}
	}

	public RegexOptions Options
	{
		get
		{
			throw null;
		}
	}

	public bool RightToLeft
	{
		get
		{
			throw null;
		}
	}

	protected Regex()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Regex(SerializationInfo info, StreamingContext context)
	{
	}

	public Regex([StringSyntax("Regex")] string pattern)
	{
	}

	public Regex([StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
	}

	public Regex([StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
	}

	[Obsolete("Regex.CompileToAssembly is obsolete and not supported. Use the GeneratedRegexAttribute with the regular expression source generator instead.", DiagnosticId = "SYSLIB0036", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname)
	{
	}

	[Obsolete("Regex.CompileToAssembly is obsolete and not supported. Use the GeneratedRegexAttribute with the regular expression source generator instead.", DiagnosticId = "SYSLIB0036", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[]? attributes)
	{
	}

	[Obsolete("Regex.CompileToAssembly is obsolete and not supported. Use the GeneratedRegexAttribute with the regular expression source generator instead.", DiagnosticId = "SYSLIB0036", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[]? attributes, string? resourceFile)
	{
	}

	public int Count(string input)
	{
		throw null;
	}

	public int Count(ReadOnlySpan<char> input)
	{
		throw null;
	}

	public int Count(ReadOnlySpan<char> input, int startat)
	{
		throw null;
	}

	public static int Count(string input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static int Count(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static int Count(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public static int Count(ReadOnlySpan<char> input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static int Count(ReadOnlySpan<char> input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static int Count(ReadOnlySpan<char> input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public static string Escape(string str)
	{
		throw null;
	}

	public ValueMatchEnumerator EnumerateMatches(ReadOnlySpan<char> input)
	{
		throw null;
	}

	public ValueMatchEnumerator EnumerateMatches(ReadOnlySpan<char> input, int startat)
	{
		throw null;
	}

	public static ValueMatchEnumerator EnumerateMatches(ReadOnlySpan<char> input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static ValueMatchEnumerator EnumerateMatches(ReadOnlySpan<char> input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static ValueMatchEnumerator EnumerateMatches(ReadOnlySpan<char> input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public string[] GetGroupNames()
	{
		throw null;
	}

	public int[] GetGroupNumbers()
	{
		throw null;
	}

	public string GroupNameFromNumber(int i)
	{
		throw null;
	}

	public int GroupNumberFromName(string name)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete mechanisms for Regex extensibility. It is not supported.", DiagnosticId = "SYSLIB0052", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void InitializeReferences()
	{
	}

	public bool IsMatch(ReadOnlySpan<char> input)
	{
		throw null;
	}

	public bool IsMatch(ReadOnlySpan<char> input, int startat)
	{
		throw null;
	}

	public static bool IsMatch(ReadOnlySpan<char> input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static bool IsMatch(ReadOnlySpan<char> input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static bool IsMatch(ReadOnlySpan<char> input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public bool IsMatch(string input)
	{
		throw null;
	}

	public bool IsMatch(string input, int startat)
	{
		throw null;
	}

	public static bool IsMatch(string input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static bool IsMatch(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static bool IsMatch(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public Match Match(string input)
	{
		throw null;
	}

	public Match Match(string input, int startat)
	{
		throw null;
	}

	public Match Match(string input, int beginning, int length)
	{
		throw null;
	}

	public static Match Match(string input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static Match Match(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static Match Match(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public MatchCollection Matches(string input)
	{
		throw null;
	}

	public MatchCollection Matches(string input, int startat)
	{
		throw null;
	}

	public static MatchCollection Matches(string input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static MatchCollection Matches(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static MatchCollection Matches(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public string Replace(string input, string replacement)
	{
		throw null;
	}

	public string Replace(string input, string replacement, int count)
	{
		throw null;
	}

	public string Replace(string input, string replacement, int count, int startat)
	{
		throw null;
	}

	public static string Replace(string input, [StringSyntax("Regex")] string pattern, string replacement)
	{
		throw null;
	}

	public static string Replace(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, string replacement, RegexOptions options)
	{
		throw null;
	}

	public static string Replace(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, string replacement, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public static string Replace(string input, [StringSyntax("Regex")] string pattern, MatchEvaluator evaluator)
	{
		throw null;
	}

	public static string Replace(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, MatchEvaluator evaluator, RegexOptions options)
	{
		throw null;
	}

	public static string Replace(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	public string Replace(string input, MatchEvaluator evaluator)
	{
		throw null;
	}

	public string Replace(string input, MatchEvaluator evaluator, int count)
	{
		throw null;
	}

	public string Replace(string input, MatchEvaluator evaluator, int count, int startat)
	{
		throw null;
	}

	public string[] Split(string input)
	{
		throw null;
	}

	public string[] Split(string input, int count)
	{
		throw null;
	}

	public string[] Split(string input, int count, int startat)
	{
		throw null;
	}

	public static string[] Split(string input, [StringSyntax("Regex")] string pattern)
	{
		throw null;
	}

	public static string[] Split(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options)
	{
		throw null;
	}

	public static string[] Split(string input, [StringSyntax("Regex", new object[] { "options" })] string pattern, RegexOptions options, TimeSpan matchTimeout)
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public static string Unescape(string str)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete mechanisms for Regex extensibility. It is not supported.", DiagnosticId = "SYSLIB0052", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected bool UseOptionC()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete mechanisms for Regex extensibility. It is not supported.", DiagnosticId = "SYSLIB0052", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected bool UseOptionR()
	{
		throw null;
	}

	protected internal static void ValidateMatchTimeout(TimeSpan matchTimeout)
	{
	}
}
