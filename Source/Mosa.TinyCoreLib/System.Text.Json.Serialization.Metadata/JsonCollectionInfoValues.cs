using System.ComponentModel;

namespace System.Text.Json.Serialization.Metadata;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class JsonCollectionInfoValues<TCollection>
{
	public JsonTypeInfo ElementInfo
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public JsonTypeInfo? KeyInfo
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

	public Func<TCollection>? ObjectCreator
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Action<Utf8JsonWriter, TCollection>? SerializeHandler
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
