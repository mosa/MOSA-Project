using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json.Serialization;

public abstract class JsonConverter
{
	public abstract Type? Type { get; }

	internal JsonConverter()
	{
	}

	public abstract bool CanConvert(Type typeToConvert);
}
public abstract class JsonConverter<T> : JsonConverter
{
	public virtual bool HandleNull
	{
		get
		{
			throw null;
		}
	}

	public sealed override Type Type
	{
		get
		{
			throw null;
		}
	}

	protected internal JsonConverter()
	{
	}

	public override bool CanConvert(Type typeToConvert)
	{
		throw null;
	}

	public abstract T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);

	public virtual T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw null;
	}

	public abstract void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);

	public virtual void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] T value, JsonSerializerOptions options)
	{
	}
}
