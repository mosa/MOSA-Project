using System.ComponentModel;

namespace System.Text.Json.Serialization.Metadata;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class JsonPropertyInfoValues<T>
{
	public JsonConverter<T>? Converter
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Type DeclaringType
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Func<object, T?>? Getter
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public bool HasJsonInclude
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public JsonIgnoreCondition? IgnoreCondition
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public bool IsExtensionData
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public bool IsProperty
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public bool IsPublic
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public bool IsVirtual
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public string? JsonPropertyName
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public JsonNumberHandling? NumberHandling
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public string PropertyName
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public JsonTypeInfo PropertyTypeInfo
	{
		get
		{
			throw null;
		}
		init
		{
		}
	}

	public Action<object, T?>? Setter
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
