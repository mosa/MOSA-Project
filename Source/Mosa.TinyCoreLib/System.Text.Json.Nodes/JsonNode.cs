using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json.Nodes;

public abstract class JsonNode
{
	public JsonNode? this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonNode? this[string propertyName]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonNodeOptions? Options
	{
		get
		{
			throw null;
		}
	}

	public JsonNode? Parent
	{
		get
		{
			throw null;
		}
	}

	public JsonNode Root
	{
		get
		{
			throw null;
		}
	}

	internal JsonNode()
	{
	}

	public JsonArray AsArray()
	{
		throw null;
	}

	public JsonObject AsObject()
	{
		throw null;
	}

	public JsonValue AsValue()
	{
		throw null;
	}

	public JsonNode DeepClone()
	{
		throw null;
	}

	public static bool DeepEquals(JsonNode? node1, JsonNode? node2)
	{
		throw null;
	}

	public string GetPropertyName()
	{
		throw null;
	}

	public string GetPath()
	{
		throw null;
	}

	public virtual T GetValue<T>()
	{
		throw null;
	}

	public JsonValueKind GetValueKind()
	{
		throw null;
	}

	public int GetElementIndex()
	{
		throw null;
	}

	public static explicit operator bool(JsonNode value)
	{
		throw null;
	}

	public static explicit operator byte(JsonNode value)
	{
		throw null;
	}

	public static explicit operator char(JsonNode value)
	{
		throw null;
	}

	public static explicit operator DateTime(JsonNode value)
	{
		throw null;
	}

	public static explicit operator DateTimeOffset(JsonNode value)
	{
		throw null;
	}

	public static explicit operator decimal(JsonNode value)
	{
		throw null;
	}

	public static explicit operator double(JsonNode value)
	{
		throw null;
	}

	public static explicit operator Guid(JsonNode value)
	{
		throw null;
	}

	public static explicit operator short(JsonNode value)
	{
		throw null;
	}

	public static explicit operator int(JsonNode value)
	{
		throw null;
	}

	public static explicit operator long(JsonNode value)
	{
		throw null;
	}

	public static explicit operator bool?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator byte?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator char?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator DateTimeOffset?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator DateTime?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator decimal?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator double?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator Guid?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator short?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator int?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator long?(JsonNode? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte?(JsonNode? value)
	{
		throw null;
	}

	public static explicit operator float?(JsonNode? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort?(JsonNode? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint?(JsonNode? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong?(JsonNode? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator sbyte(JsonNode value)
	{
		throw null;
	}

	public static explicit operator float(JsonNode value)
	{
		throw null;
	}

	public static explicit operator string?(JsonNode? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ushort(JsonNode value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(JsonNode value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(JsonNode value)
	{
		throw null;
	}

	public static implicit operator JsonNode(bool value)
	{
		throw null;
	}

	public static implicit operator JsonNode(byte value)
	{
		throw null;
	}

	public static implicit operator JsonNode(char value)
	{
		throw null;
	}

	public static implicit operator JsonNode(DateTime value)
	{
		throw null;
	}

	public static implicit operator JsonNode(DateTimeOffset value)
	{
		throw null;
	}

	public static implicit operator JsonNode(decimal value)
	{
		throw null;
	}

	public static implicit operator JsonNode(double value)
	{
		throw null;
	}

	public static implicit operator JsonNode(Guid value)
	{
		throw null;
	}

	public static implicit operator JsonNode(short value)
	{
		throw null;
	}

	public static implicit operator JsonNode(int value)
	{
		throw null;
	}

	public static implicit operator JsonNode(long value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(bool? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(byte? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(char? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(DateTimeOffset? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(DateTime? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(decimal? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(double? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(Guid? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(short? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(int? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(long? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode?(sbyte? value)
	{
		throw null;
	}

	public static implicit operator JsonNode?(float? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode?(ushort? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode?(uint? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode?(ulong? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode(sbyte value)
	{
		throw null;
	}

	public static implicit operator JsonNode(float value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	public static implicit operator JsonNode?(string? value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static implicit operator JsonNode(ulong value)
	{
		throw null;
	}

	public static JsonNode? Parse(Stream utf8Json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static JsonNode? Parse(ReadOnlySpan<byte> utf8Json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static JsonNode? Parse([StringSyntax("Json")] string json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default(JsonDocumentOptions))
	{
		throw null;
	}

	public static JsonNode? Parse(ref Utf8JsonReader reader, JsonNodeOptions? nodeOptions = null)
	{
		throw null;
	}

	public static Task<JsonNode?> ParseAsync(Stream utf8Json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default(JsonDocumentOptions), CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[RequiresDynamicCode("Creating JsonValue instances with non-primitive types requires generating code at runtime.")]
	[RequiresUnreferencedCode("Creating JsonValue instances with non-primitive types is not compatible with trimming. It can result in non-primitive types being serialized, which may have their members trimmed.")]
	public void ReplaceWith<T>(T value)
	{
		throw null;
	}

	public string ToJsonString(JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public abstract void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null);
}
