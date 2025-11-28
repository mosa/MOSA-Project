using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;

namespace System.Text.Json;

public readonly struct JsonEncodedText : IEquatable<JsonEncodedText>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ReadOnlySpan<byte> EncodedUtf8Bytes
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
	}

	public static JsonEncodedText Encode(ReadOnlySpan<byte> utf8Value, JavaScriptEncoder? encoder = null)
	{
		throw null;
	}

	public static JsonEncodedText Encode(ReadOnlySpan<char> value, JavaScriptEncoder? encoder = null)
	{
		throw null;
	}

	public static JsonEncodedText Encode(string value, JavaScriptEncoder? encoder = null)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(JsonEncodedText other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
