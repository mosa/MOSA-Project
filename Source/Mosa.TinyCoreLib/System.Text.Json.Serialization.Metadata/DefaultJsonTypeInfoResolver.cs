using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json.Serialization.Metadata;

public class DefaultJsonTypeInfoResolver : IJsonTypeInfoResolver
{
	public IList<Action<JsonTypeInfo>> Modifiers
	{
		get
		{
			throw null;
		}
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
	public DefaultJsonTypeInfoResolver()
	{
	}

	public virtual JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		throw null;
	}
}
