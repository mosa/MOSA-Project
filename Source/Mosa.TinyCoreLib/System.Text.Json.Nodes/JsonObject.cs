using System.Collections;
using System.Collections.Generic;

namespace System.Text.Json.Nodes;

public sealed class JsonObject : JsonNode, ICollection<KeyValuePair<string, JsonNode?>>, IEnumerable<KeyValuePair<string, JsonNode?>>, IEnumerable, IDictionary<string, JsonNode?>
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<KeyValuePair<string, JsonNode>>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	ICollection<string> IDictionary<string, JsonNode>.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection<JsonNode?> IDictionary<string, JsonNode>.Values
	{
		get
		{
			throw null;
		}
	}

	public JsonObject(IEnumerable<KeyValuePair<string, JsonNode?>> properties, JsonNodeOptions? options = null)
	{
	}

	public JsonObject(JsonNodeOptions? options = null)
	{
	}

	public void Add(KeyValuePair<string, JsonNode?> property)
	{
	}

	public void Add(string propertyName, JsonNode? value)
	{
	}

	public void Clear()
	{
	}

	public bool ContainsKey(string propertyName)
	{
		throw null;
	}

	public static JsonObject? Create(JsonElement element, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public IEnumerator<KeyValuePair<string, JsonNode?>> GetEnumerator()
	{
		throw null;
	}

	public bool Remove(string propertyName)
	{
		throw null;
	}

	bool ICollection<KeyValuePair<string, JsonNode>>.Contains(KeyValuePair<string, JsonNode> item)
	{
		throw null;
	}

	void ICollection<KeyValuePair<string, JsonNode>>.CopyTo(KeyValuePair<string, JsonNode>[] array, int index)
	{
	}

	bool ICollection<KeyValuePair<string, JsonNode>>.Remove(KeyValuePair<string, JsonNode> item)
	{
		throw null;
	}

	bool IDictionary<string, JsonNode>.TryGetValue(string propertyName, out JsonNode? jsonNode)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public bool TryGetPropertyValue(string propertyName, out JsonNode? jsonNode)
	{
		throw null;
	}

	public override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null)
	{
	}
}
