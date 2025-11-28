using System.ComponentModel;

namespace System.Text.Json.Serialization.Metadata;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class JsonObjectInfoValues<T>
{
	public Func<JsonParameterInfoValues[]>? ConstructorParameterMetadataInitializer
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public JsonNumberHandling NumberHandling
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Func<T>? ObjectCreator
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Func<object[], T>? ObjectWithParameterizedConstructorCreator
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Func<JsonSerializerContext, JsonPropertyInfo[]>? PropertyMetadataInitializer
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Action<Utf8JsonWriter, T>? SerializeHandler
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}
}
