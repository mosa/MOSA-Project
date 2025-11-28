namespace System.Text.Json;

public readonly struct JsonProperty
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public JsonElement Value
	{
		get
		{
			throw null;
		}
	}

	public bool NameEquals(ReadOnlySpan<byte> utf8Text)
	{
		throw null;
	}

	public bool NameEquals(ReadOnlySpan<char> text)
	{
		throw null;
	}

	public bool NameEquals(string? text)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public void WriteTo(Utf8JsonWriter writer)
	{
	}
}
