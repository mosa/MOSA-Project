using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace System;

[JsonConverter(typeof(BinaryDataJsonConverter))]
public class BinaryData
{
	public static BinaryData Empty
	{
		get
		{
			throw null;
		}
	}

	public bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public string? MediaType
	{
		get
		{
			throw null;
		}
	}

	public BinaryData(byte[] data)
	{
	}

	public BinaryData(byte[] data, string? mediaType)
	{
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
	public BinaryData(object? jsonSerializable, JsonSerializerOptions? options = null, Type? type = null)
	{
	}

	public BinaryData(object? jsonSerializable, JsonSerializerContext context, Type? type = null)
	{
	}

	public BinaryData(ReadOnlyMemory<byte> data)
	{
	}

	public BinaryData(ReadOnlyMemory<byte> data, string? mediaType)
	{
	}

	public BinaryData(string data)
	{
	}

	public BinaryData(string data, string? mediaType)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static BinaryData FromBytes(byte[] data)
	{
		throw null;
	}

	public static BinaryData FromBytes(byte[] data, string? mediaType)
	{
		throw null;
	}

	public static BinaryData FromBytes(ReadOnlyMemory<byte> data)
	{
		throw null;
	}

	public static BinaryData FromBytes(ReadOnlyMemory<byte> data, string? mediaType)
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
	public static BinaryData FromObjectAsJson<T>(T jsonSerializable, JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public static BinaryData FromObjectAsJson<T>(T jsonSerializable, JsonTypeInfo<T> jsonTypeInfo)
	{
		throw null;
	}

	public static BinaryData FromStream(Stream stream)
	{
		throw null;
	}

	public static BinaryData FromStream(Stream stream, string? mediaType)
	{
		throw null;
	}

	public static Task<BinaryData> FromStreamAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<BinaryData> FromStreamAsync(Stream stream, string? mediaType, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static BinaryData FromString(string data)
	{
		throw null;
	}

	public static BinaryData FromString(string data, string? mediaType)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		throw null;
	}

	public static implicit operator ReadOnlyMemory<byte>(BinaryData? data)
	{
		throw null;
	}

	public static implicit operator ReadOnlySpan<byte>(BinaryData? data)
	{
		throw null;
	}

	public byte[] ToArray()
	{
		throw null;
	}

	public ReadOnlyMemory<byte> ToMemory()
	{
		throw null;
	}

	[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation.")]
	[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
	public T? ToObjectFromJson<T>(JsonSerializerOptions? options = null)
	{
		throw null;
	}

	public T? ToObjectFromJson<T>(JsonTypeInfo<T> jsonTypeInfo)
	{
		throw null;
	}

	public Stream ToStream()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public BinaryData WithMediaType(string? mediaType)
	{
		throw null;
	}
}
