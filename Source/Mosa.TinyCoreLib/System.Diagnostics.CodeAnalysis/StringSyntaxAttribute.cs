namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class StringSyntaxAttribute : Attribute
{
	public const string CompositeFormat = "CompositeFormat";

	public const string DateOnlyFormat = "DateOnlyFormat";

	public const string DateTimeFormat = "DateTimeFormat";

	public const string EnumFormat = "EnumFormat";

	public const string GuidFormat = "GuidFormat";

	public const string Json = "Json";

	public const string NumericFormat = "NumericFormat";

	public const string Regex = "Regex";

	public const string TimeOnlyFormat = "TimeOnlyFormat";

	public const string TimeSpanFormat = "TimeSpanFormat";

	public const string Uri = "Uri";

	public const string Xml = "Xml";

	public string Syntax
	{
		get
		{
			throw null;
		}
	}

	public object?[] Arguments
	{
		get
		{
			throw null;
		}
	}

	public StringSyntaxAttribute(string syntax)
	{
	}

	public StringSyntaxAttribute(string syntax, params object?[] arguments)
	{
	}
}
