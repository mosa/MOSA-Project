namespace System.Text.Json.Serialization.Metadata;

public static class JsonTypeInfoResolver
{
	public static IJsonTypeInfoResolver Combine(params IJsonTypeInfoResolver?[] resolvers)
	{
		throw null;
	}

	public static IJsonTypeInfoResolver WithAddedModifier(this IJsonTypeInfoResolver resolver, Action<JsonTypeInfo> modifier)
	{
		throw null;
	}
}
