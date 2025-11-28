using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.ComponentModel.Composition.ReflectionModel;

public static class ReflectionModelServices
{
	public static ExportDefinition CreateExportDefinition(LazyMemberInfo exportingMember, string contractName, Lazy<IDictionary<string, object?>> metadata, ICompositionElement? origin)
	{
		throw null;
	}

	public static ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string? requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPreRequisite, CreationPolicy requiredCreationPolicy, IDictionary<string, object?> metadata, bool isExportFactory, ICompositionElement? origin)
	{
		throw null;
	}

	public static ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string? requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, bool isRecomposable, CreationPolicy requiredCreationPolicy, IDictionary<string, object?> metadata, bool isExportFactory, ICompositionElement? origin)
	{
		throw null;
	}

	public static ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string? requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, bool isRecomposable, CreationPolicy requiredCreationPolicy, ICompositionElement? origin)
	{
		throw null;
	}

	public static ContractBasedImportDefinition CreateImportDefinition(Lazy<ParameterInfo> parameter, string contractName, string? requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, CreationPolicy requiredCreationPolicy, IDictionary<string, object?> metadata, bool isExportFactory, ICompositionElement? origin)
	{
		throw null;
	}

	public static ContractBasedImportDefinition CreateImportDefinition(Lazy<ParameterInfo> parameter, string contractName, string? requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, CreationPolicy requiredCreationPolicy, ICompositionElement? origin)
	{
		throw null;
	}

	public static ComposablePartDefinition CreatePartDefinition(Lazy<Type> partType, bool isDisposalRequired, Lazy<IEnumerable<ImportDefinition>>? imports, Lazy<IEnumerable<ExportDefinition>>? exports, Lazy<IDictionary<string, object?>>? metadata, ICompositionElement? origin)
	{
		throw null;
	}

	public static ContractBasedImportDefinition GetExportFactoryProductImportDefinition(ImportDefinition importDefinition)
	{
		throw null;
	}

	public static LazyMemberInfo GetExportingMember(ExportDefinition exportDefinition)
	{
		throw null;
	}

	public static LazyMemberInfo GetImportingMember(ImportDefinition importDefinition)
	{
		throw null;
	}

	public static Lazy<ParameterInfo> GetImportingParameter(ImportDefinition importDefinition)
	{
		throw null;
	}

	public static Lazy<Type> GetPartType(ComposablePartDefinition partDefinition)
	{
		throw null;
	}

	public static bool IsDisposalRequired(ComposablePartDefinition partDefinition)
	{
		throw null;
	}

	public static bool IsExportFactoryImportDefinition(ImportDefinition importDefinition)
	{
		throw null;
	}

	public static bool IsImportingParameter(ImportDefinition importDefinition)
	{
		throw null;
	}

	public static bool TryMakeGenericPartDefinition(ComposablePartDefinition partDefinition, IEnumerable<Type> genericParameters, [NotNullWhen(true)] out ComposablePartDefinition? specialization)
	{
		throw null;
	}
}
