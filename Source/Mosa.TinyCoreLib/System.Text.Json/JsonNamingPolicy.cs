namespace System.Text.Json;

public abstract class JsonNamingPolicy
{
	public static JsonNamingPolicy CamelCase
	{
		get
		{
			throw null;
		}
	}

	public static JsonNamingPolicy KebabCaseLower
	{
		get
		{
			throw null;
		}
	}

	public static JsonNamingPolicy KebabCaseUpper
	{
		get
		{
			throw null;
		}
	}

	public static JsonNamingPolicy SnakeCaseLower
	{
		get
		{
			throw null;
		}
	}

	public static JsonNamingPolicy SnakeCaseUpper
	{
		get
		{
			throw null;
		}
	}

	public abstract string ConvertName(string name);
}
