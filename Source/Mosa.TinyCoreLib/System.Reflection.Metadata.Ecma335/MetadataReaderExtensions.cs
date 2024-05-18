using System.Collections.Generic;

namespace System.Reflection.Metadata.Ecma335;

public static class MetadataReaderExtensions
{
	public static IEnumerable<EditAndContinueLogEntry> GetEditAndContinueLogEntries(this MetadataReader reader)
	{
		throw null;
	}

	public static IEnumerable<EntityHandle> GetEditAndContinueMapEntries(this MetadataReader reader)
	{
		throw null;
	}

	public static int GetHeapMetadataOffset(this MetadataReader reader, HeapIndex heapIndex)
	{
		throw null;
	}

	public static int GetHeapSize(this MetadataReader reader, HeapIndex heapIndex)
	{
		throw null;
	}

	public static BlobHandle GetNextHandle(this MetadataReader reader, BlobHandle handle)
	{
		throw null;
	}

	public static StringHandle GetNextHandle(this MetadataReader reader, StringHandle handle)
	{
		throw null;
	}

	public static UserStringHandle GetNextHandle(this MetadataReader reader, UserStringHandle handle)
	{
		throw null;
	}

	public static int GetTableMetadataOffset(this MetadataReader reader, TableIndex tableIndex)
	{
		throw null;
	}

	public static int GetTableRowCount(this MetadataReader reader, TableIndex tableIndex)
	{
		throw null;
	}

	public static int GetTableRowSize(this MetadataReader reader, TableIndex tableIndex)
	{
		throw null;
	}

	public static IEnumerable<TypeDefinitionHandle> GetTypesWithEvents(this MetadataReader reader)
	{
		throw null;
	}

	public static IEnumerable<TypeDefinitionHandle> GetTypesWithProperties(this MetadataReader reader)
	{
		throw null;
	}

	public static SignatureTypeKind ResolveSignatureTypeKind(this MetadataReader reader, EntityHandle typeHandle, byte rawTypeKind)
	{
		throw null;
	}
}
