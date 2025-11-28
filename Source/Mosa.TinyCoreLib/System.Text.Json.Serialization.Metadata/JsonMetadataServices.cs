using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Nodes;

namespace System.Text.Json.Serialization.Metadata;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class JsonMetadataServices
{
	public static JsonConverter<bool> BooleanConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<byte[]?> ByteArrayConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<byte> ByteConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<char> CharConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<DateTime> DateTimeConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<DateTimeOffset> DateTimeOffsetConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<decimal> DecimalConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<double> DoubleConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<Guid> GuidConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<short> Int16Converter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<int> Int32Converter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<long> Int64Converter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<JsonArray?> JsonArrayConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<JsonDocument?> JsonDocumentConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<JsonElement> JsonElementConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<JsonNode?> JsonNodeConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<JsonObject?> JsonObjectConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<JsonValue?> JsonValueConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<Memory<byte>> MemoryByteConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<object?> ObjectConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<ReadOnlyMemory<byte>> ReadOnlyMemoryByteConverter
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public static JsonConverter<sbyte> SByteConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<float> SingleConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<string?> StringConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<TimeSpan> TimeSpanConverter
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public static JsonConverter<ushort> UInt16Converter
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public static JsonConverter<uint> UInt32Converter
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public static JsonConverter<ulong> UInt64Converter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<Uri?> UriConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<Version?> VersionConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<DateOnly> DateOnlyConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<Half> HalfConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonConverter<TimeOnly> TimeOnlyConverter
	{
		get
		{
			throw null;
		}
	}

	public static JsonTypeInfo<TElement[]> CreateArrayInfo<TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TElement[]> collectionInfo)
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateConcurrentQueueInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : ConcurrentQueue<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateConcurrentStackInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : ConcurrentStack<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : Dictionary<TKey, TValue> where TKey : notnull
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIAsyncEnumerableInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IAsyncEnumerable<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateICollectionInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : ICollection<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIDictionaryInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IDictionary
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IDictionary<TKey, TValue> where TKey : notnull
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIEnumerableInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IEnumerable
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIEnumerableInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IEnumerable<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIListInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IList
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIListInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IList<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateImmutableDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Func<IEnumerable<KeyValuePair<TKey, TValue>>, TCollection> createRangeFunc) where TCollection : IReadOnlyDictionary<TKey, TValue> where TKey : notnull
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateImmutableEnumerableInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Func<IEnumerable<TElement>, TCollection> createRangeFunc) where TCollection : IEnumerable<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateIReadOnlyDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : IReadOnlyDictionary<TKey, TValue> where TKey : notnull
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateISetInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : ISet<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateListInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : List<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<Memory<TElement>> CreateMemoryInfo<TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<Memory<TElement>> collectionInfo)
	{
		throw null;
	}

	public static JsonTypeInfo<T> CreateObjectInfo<T>(JsonSerializerOptions options, JsonObjectInfoValues<T> objectInfo) where T : notnull
	{
		throw null;
	}

	public static JsonPropertyInfo CreatePropertyInfo<T>(JsonSerializerOptions options, JsonPropertyInfoValues<T> propertyInfo)
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateQueueInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Action<TCollection, object?> addFunc) where TCollection : IEnumerable
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateQueueInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : Queue<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<ReadOnlyMemory<TElement>> CreateReadOnlyMemoryInfo<TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<ReadOnlyMemory<TElement>> collectionInfo)
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateStackInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Action<TCollection, object?> addFunc) where TCollection : IEnumerable
	{
		throw null;
	}

	public static JsonTypeInfo<TCollection> CreateStackInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo) where TCollection : Stack<TElement>
	{
		throw null;
	}

	public static JsonTypeInfo<T> CreateValueInfo<T>(JsonSerializerOptions options, JsonConverter converter)
	{
		throw null;
	}

	public static JsonConverter<T> GetEnumConverter<T>(JsonSerializerOptions options) where T : struct, Enum
	{
		throw null;
	}

	public static JsonConverter<T?> GetNullableConverter<T>(JsonSerializerOptions options) where T : struct
	{
		throw null;
	}

	public static JsonConverter<T?> GetNullableConverter<T>(JsonTypeInfo<T> underlyingTypeInfo) where T : struct
	{
		throw null;
	}

	public static JsonConverter<T> GetUnsupportedTypeConverter<T>()
	{
		throw null;
	}
}
