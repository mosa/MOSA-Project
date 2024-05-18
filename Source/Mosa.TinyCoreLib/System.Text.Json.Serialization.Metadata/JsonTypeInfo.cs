using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json.Serialization.Metadata;

public abstract class JsonTypeInfo
{
	public JsonConverter Converter
	{
		get
		{
			throw null;
		}
	}

	public Func<object>? CreateObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public JsonTypeInfoKind Kind
	{
		get
		{
			throw null;
		}
	}

	public JsonNumberHandling? NumberHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Action<object>? OnDeserialized
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Action<object>? OnDeserializing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Action<object>? OnSerialized
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Action<object>? OnSerializing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonSerializerOptions Options
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public IJsonTypeInfoResolver? OriginatingResolver
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonPolymorphismOptions? PolymorphismOptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonObjectCreationHandling? PreferredPropertyObjectCreationHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IList<JsonPropertyInfo> Properties
	{
		get
		{
			throw null;
		}
	}

	public Type Type
	{
		get
		{
			throw null;
		}
	}

	public JsonUnmappedMemberHandling? UnmappedMemberHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal JsonTypeInfo()
	{
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	public JsonPropertyInfo CreateJsonPropertyInfo(Type propertyType, string name)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	public static JsonTypeInfo CreateJsonTypeInfo(Type type, JsonSerializerOptions options)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	public static JsonTypeInfo<T> CreateJsonTypeInfo<T>(JsonSerializerOptions options)
	{
		throw null;
	}

	public void MakeReadOnly()
	{
	}
}
public sealed class JsonTypeInfo<T> : JsonTypeInfo
{
	public new Func<T>? CreateObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Action<Utf8JsonWriter, T>? SerializeHandler
	{
		get
		{
			throw null;
		}
	}

	internal JsonTypeInfo()
	{
	}
}
