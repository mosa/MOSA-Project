using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;

namespace System.Text.Json.Nodes;

public abstract class JsonValue : JsonNode
{
	internal JsonValue()
	{
	}

	public static JsonValue Create(bool value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(byte value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(char value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(DateTime value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(DateTimeOffset value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(decimal value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(double value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(Guid value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(short value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(int value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(long value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(bool? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(byte? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(char? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(DateTimeOffset? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(DateTime? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(decimal? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(double? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(Guid? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(short? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(int? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(long? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue? Create(sbyte? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(float? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(JsonElement? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue? Create(ushort? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue? Create(uint? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue? Create(ulong? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue Create(sbyte value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue Create(float value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static JsonValue? Create(string? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create(JsonElement value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue Create(ushort value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue Create(uint value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static JsonValue Create(ulong value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	[RequiresDynamicCode("Creating JsonValue instances with non-primitive types requires generating code at runtime.")]
	[RequiresUnreferencedCode("Creating JsonValue instances with non-primitive types is not compatible with trimming. It can result in non-primitive types being serialized, which may have their members trimmed. Use the overload that takes a JsonTypeInfo, or make sure all of the required types are preserved.")]
	public static JsonValue? Create<T>(T? value, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public static JsonValue? Create<T>(T? value, JsonTypeInfo<T> jsonTypeInfo, JsonNodeOptions? options = null)
	{
		throw null;
	}

	public abstract bool TryGetValue<T>([NotNullWhen(true)] out T? value);
}
