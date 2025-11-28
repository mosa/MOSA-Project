using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.ComponentModel.Composition.Primitives;

public class ContractBasedImportDefinition : ImportDefinition
{
	public override Expression<Func<ExportDefinition, bool>> Constraint
	{
		get
		{
			throw null;
		}
	}

	public virtual CreationPolicy RequiredCreationPolicy
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<KeyValuePair<string, Type>> RequiredMetadata
	{
		get
		{
			throw null;
		}
	}

	public virtual string? RequiredTypeIdentity
	{
		get
		{
			throw null;
		}
	}

	protected ContractBasedImportDefinition()
	{
	}

	public ContractBasedImportDefinition(string contractName, string? requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy)
	{
	}

	public ContractBasedImportDefinition(string contractName, string? requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy, IDictionary<string, object?> metadata)
	{
	}

	public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
