using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace System.ComponentModel.Composition;

public static class AttributedModelServices
{
	public static ComposablePart AddExportedValue<T>(this CompositionBatch batch, string contractName, T exportedValue)
	{
		throw null;
	}

	public static ComposablePart AddExportedValue<T>(this CompositionBatch batch, T exportedValue)
	{
		throw null;
	}

	public static ComposablePart AddPart(this CompositionBatch batch, object attributedPart)
	{
		throw null;
	}

	public static void ComposeExportedValue<T>(this CompositionContainer container, string contractName, T exportedValue)
	{
	}

	public static void ComposeExportedValue<T>(this CompositionContainer container, T exportedValue)
	{
	}

	public static void ComposeParts(this CompositionContainer container, params object[] attributedParts)
	{
	}

	public static ComposablePart CreatePart(ComposablePartDefinition partDefinition, object attributedPart)
	{
		throw null;
	}

	public static ComposablePart CreatePart(object attributedPart)
	{
		throw null;
	}

	public static ComposablePart CreatePart(object attributedPart, ReflectionContext reflectionContext)
	{
		throw null;
	}

	public static ComposablePartDefinition CreatePartDefinition(Type type, ICompositionElement? origin)
	{
		throw null;
	}

	public static ComposablePartDefinition CreatePartDefinition(Type type, ICompositionElement? origin, bool ensureIsDiscoverable)
	{
		throw null;
	}

	public static bool Exports(this ComposablePartDefinition part, Type contractType)
	{
		throw null;
	}

	public static bool Exports<T>(this ComposablePartDefinition part)
	{
		throw null;
	}

	public static string GetContractName(Type type)
	{
		throw null;
	}

	public static TMetadataView GetMetadataView<TMetadataView>(IDictionary<string, object?> metadata)
	{
		throw null;
	}

	public static string GetTypeIdentity(MethodInfo method)
	{
		throw null;
	}

	public static string GetTypeIdentity(Type type)
	{
		throw null;
	}

	public static bool Imports(this ComposablePartDefinition part, Type contractType)
	{
		throw null;
	}

	public static bool Imports(this ComposablePartDefinition part, Type contractType, ImportCardinality importCardinality)
	{
		throw null;
	}

	public static bool Imports<T>(this ComposablePartDefinition part)
	{
		throw null;
	}

	public static bool Imports<T>(this ComposablePartDefinition part, ImportCardinality importCardinality)
	{
		throw null;
	}

	public static ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart)
	{
		throw null;
	}

	public static ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart, ReflectionContext reflectionContext)
	{
		throw null;
	}
}
