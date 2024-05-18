using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.ComponentModel.Composition.Primitives;

public class ImportDefinition
{
	public virtual ImportCardinality Cardinality
	{
		get
		{
			throw null;
		}
	}

	public virtual Expression<Func<ExportDefinition, bool>> Constraint
	{
		get
		{
			throw null;
		}
	}

	public virtual string ContractName
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsPrerequisite
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsRecomposable
	{
		get
		{
			throw null;
		}
	}

	public virtual IDictionary<string, object?> Metadata
	{
		get
		{
			throw null;
		}
	}

	protected ImportDefinition()
	{
	}

	public ImportDefinition(Expression<Func<ExportDefinition, bool>> constraint, string? contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite)
	{
	}

	public ImportDefinition(Expression<Func<ExportDefinition, bool>> constraint, string? contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, IDictionary<string, object?>? metadata)
	{
	}

	public virtual bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
