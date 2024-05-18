using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public static class ScopingExtensions
{
	public static bool ContainsPartMetadataWithKey(this ComposablePartDefinition part, string key)
	{
		throw null;
	}

	public static bool ContainsPartMetadata<T>(this ComposablePartDefinition part, string key, T value)
	{
		throw null;
	}

	public static bool Exports(this ComposablePartDefinition part, string contractName)
	{
		throw null;
	}

	public static FilteredCatalog Filter(this ComposablePartCatalog catalog, Func<ComposablePartDefinition, bool> filter)
	{
		throw null;
	}

	public static bool Imports(this ComposablePartDefinition part, string contractName)
	{
		throw null;
	}

	public static bool Imports(this ComposablePartDefinition part, string contractName, ImportCardinality importCardinality)
	{
		throw null;
	}
}
