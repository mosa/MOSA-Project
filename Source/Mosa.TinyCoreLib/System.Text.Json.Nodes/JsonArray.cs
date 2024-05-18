using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json.Nodes;

public sealed class JsonArray : JsonNode, ICollection<JsonNode?>, IEnumerable<JsonNode?>, IEnumerable, IList<JsonNode?>
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<JsonNode>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public JsonArray(JsonNodeOptions? options = null)
	{
	}

	public JsonArray(JsonNodeOptions options, params JsonNode?[] items)
	{
	}

	public JsonArray(params JsonNode?[] items)
	{
	}

	public void Add(JsonNode? item)
	{
	}

	[RequiresDynamicCode("Creating JsonValue instances with non-primitive types requires generating code at runtime.")]
	[RequiresUnreferencedCode("Creating JsonValue instances with non-primitive types is not compatible with trimming. It can result in non-primitive types being serialized, which may have their members trimmed.")]
	public void Add<T>(T? value)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(JsonNode? item)
	{
		throw null;
	}

	public static JsonArray? Create(JsonElement element, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public IEnumerator<JsonNode?> GetEnumerator()
	{
		throw null;
	}

	public IEnumerable<T> GetValues<T>()
	{
		throw null;
	}

	public int IndexOf(JsonNode? item)
	{
		throw null;
	}

	public void Insert(int index, JsonNode? item)
	{
	}

	public bool Remove(JsonNode? item)
	{
		throw null;
	}

	public void RemoveAt(int index)
	{
	}

	void ICollection<JsonNode>.CopyTo(JsonNode?[]? array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null)
	{
	}
}
