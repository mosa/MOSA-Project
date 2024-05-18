using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace System.Text.Json;

public sealed class JsonSerializerOptions
{
	public bool AllowTrailingCommas
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IList<JsonConverter> Converters
	{
		get
		{
			throw null;
		}
	}

	public static JsonSerializerOptions Default
	{
		[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
		[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
		get
		{
			throw null;
		}
	}

	public int DefaultBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonIgnoreCondition DefaultIgnoreCondition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonNamingPolicy? DictionaryKeyPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JavaScriptEncoder? Encoder
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
	[Obsolete("JsonSerializerOptions.IgnoreNullValues is obsolete. To ignore null values when serializing, set DefaultIgnoreCondition to JsonIgnoreCondition.WhenWritingNull.", DiagnosticId = "SYSLIB0020", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public bool IgnoreNullValues
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IgnoreReadOnlyFields
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IgnoreReadOnlyProperties
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IncludeFields
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

	public int MaxDepth
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonNumberHandling NumberHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonObjectCreationHandling PreferredObjectCreationHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool PropertyNameCaseInsensitive
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonNamingPolicy? PropertyNamingPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonCommentHandling ReadCommentHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ReferenceHandler? ReferenceHandler
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IJsonTypeInfoResolver? TypeInfoResolver
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IList<IJsonTypeInfoResolver> TypeInfoResolverChain
	{
		get
		{
			throw null;
		}
	}

	public JsonUnknownTypeHandling UnknownTypeHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonUnmappedMemberHandling UnmappedMemberHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool WriteIndented
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonSerializerOptions()
	{
	}

	public JsonSerializerOptions(JsonSerializerDefaults defaults)
	{
	}

	public JsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("JsonSerializerOptions.AddContext is obsolete. To register a JsonSerializerContext, use either the TypeInfoResolver or TypeInfoResolverChain properties.", DiagnosticId = "SYSLIB0049", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public void AddContext<TContext>() where TContext : JsonSerializerContext, new()
	{
	}

	[RequiresDynamicCode("Getting a converter for a type may require reflection which depends on runtime code generation.")]
	[RequiresUnreferencedCode("Getting a converter for a type may require reflection which depends on unreferenced code.")]
	public JsonConverter GetConverter(Type typeToConvert)
	{
		throw null;
	}

	public JsonTypeInfo GetTypeInfo(Type type)
	{
		throw null;
	}

	public void MakeReadOnly()
	{
	}

	[RequiresDynamicCode("Populating unconfigured TypeInfoResolver properties with the reflection resolver requires runtime code generation.")]
	[RequiresUnreferencedCode("Populating unconfigured TypeInfoResolver properties with the reflection resolver requires unreferenced code.")]
	public void MakeReadOnly(bool populateMissingResolver)
	{
	}

	public bool TryGetTypeInfo(Type type, [NotNullWhen(true)] out JsonTypeInfo? typeInfo)
	{
		throw null;
	}
}
